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

// Disaridan "ayakta misin" sorusuna cevap verecek uc.
// Container orkestratorlerinin (compose, K8s) uygulamanin hazir olup olmadigini
// anlamasinin standart yolu budur.
builder.Services.AddHealthChecks();
builder.Services.AddOpenApi();

var app = builder.Build();

// Container'da 'dotnet ef' yoktur (SDK imaji calisma asamasina gecmiyor),
// bu yuzden bekleyen migration'lar acilista uygulanir.
// DbContext Scoped kayitlidir; acilista HTTP istegi -> kapsam olmadigi icin
// kapsam elle olusturulur.
// EF Core bu islem sirasinda __EFMigrationsHistory tablosunu kilitler
// (LOCK TABLE ... ACCESS EXCLUSIVE MODE), yani es zamanli kalkan kopyalar
// ayni migration'i iki kez uygulamaz.
// KALAN SINIR kilit degil, DAGITIM: eski ve yeni surum bir sure birlikte
// calisirken sema degisikligi eski surumu bozabilir. Cozum geriye donuk uyumlu
// migration yazmak; buyudukce bu adim CI/CD'de ayri bir ise tasinir.
using (var kapsam = app.Services.CreateScope())
{
    var db = kapsam.ServiceProvider.GetRequiredService<UygulamaDbContext>();
    await db.Database.MigrateAsync();
}

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

// Kimlik dogrulama GEREKTIRMEZ: saglik kontrolu token bilmez.
app.MapHealthChecks("/saglik");

app.AuthEndpointleriniEkle();
app.OyunEndpointleriniEkle();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run();