using System.Text;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(secenekler =>
    {
        var anahtar = builder.Configuration["Jwt:Anahtar"]!;

        secenekler.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Yayinci"],
            ValidAudience = builder.Configuration["Jwt:Hedef"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(anahtar)),
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