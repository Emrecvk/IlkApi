using Microsoft.EntityFrameworkCore;
using IlkApi.Endpointler;
using IlkApi.Servisler;
using IlkApi.Veri;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UygulamaDbContext>(secenekler =>
    secenekler.UseNpgsql(builder.Configuration.GetConnectionString("Varsayilan")));

builder.Services.AddScoped<IOyunServisi, OyunServisi>();

var app = builder.Build();

app.OyunEndpointleriniEkle();

app.Run();