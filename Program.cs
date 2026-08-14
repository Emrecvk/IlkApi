using FluentValidation;
using Microsoft.EntityFrameworkCore;
using IlkApi.Endpointler;
using IlkApi.Ortak;
using IlkApi.Servisler;
using IlkApi.Veri;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UygulamaDbContext>(secenekler =>
    secenekler.UseNpgsql(builder.Configuration.GetConnectionString("Varsayilan")));

builder.Services.AddScoped<IOyunServisi, OyunServisi>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddExceptionHandler<GlobalHataYakalayici>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

app.OyunEndpointleriniEkle();

app.Run();