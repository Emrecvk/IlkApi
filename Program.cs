using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using IlkApi.Dto;
using IlkApi.Dto.Dogrulayicilar;
using IlkApi.Endpointler;
using IlkApi.Ortak;
using IlkApi.Servisler;
using IlkApi.Veri;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UygulamaDbContext>(secenekler =>
    secenekler.UseNpgsql(builder.Configuration.GetConnectionString("Varsayilan")));

builder.Services.AddScoped<IOyunServisi, OyunServisi>();
builder.Services.AddScoped<IAuthServisi, AuthServisi>();
builder.Services.AddSingleton<ITokenServisi, TokenServisi>();

builder.Services.AddScoped<IValidator<OyunYaz>, OyunYazDogrulayici>();
builder.Services.AddScoped<IValidator<KayitIstek>, KayitIstekDogrulayici>();
builder.Services.AddScoped<IValidator<GirisIstek>, GirisIstekDogrulayici>();

// Ayarlar bir sinifa baglanir ve UYGULAMA ACILIRKEN dogrulanir.
// Eksik/gecersiz konfigurasyon ilk istekte degil, basta patlar (fail-fast).
builder.Services.AddOptions<JwtAyarlari>()
    .Bind(builder.Configuration.GetSection(JwtAyarlari.Bolum))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

// JwtBearer secenekleri dogrulanmis ayarlardan beslenir; konfigurasyon tek yerden okunur.
builder.Services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
    .Configure<IOptions<JwtAyarlari>>((bearerSecenekleri, jwtSecenek) =>
    {
        var ayarlar = jwtSecenek.Value;

        bearerSecenekleri.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = ayarlar.Yayinci,
            ValidAudience = ayarlar.Hedef,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ayarlar.Anahtar)),
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddExceptionHandler<GlobalHataYakalayici>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.AuthEndpointleriniEkle();
app.OyunEndpointleriniEkle();

app.Run();