using Microsoft.EntityFrameworkCore;
using IlkApi.Veri;
using IlkApi.Modeller;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UygulamaDbContext>(secenekler =>
    secenekler.UseNpgsql(builder.Configuration.GetConnectionString("Varsayilan")));

var app = builder.Build();

app.MapGet("/api/oyunlar", async (UygulamaDbContext db) =>
    await db.Oyunlar.ToListAsync());

app.MapGet("/api/oyunlar/{id:int}", async (int id, UygulamaDbContext db) =>
{
    var oyun = await db.Oyunlar.FindAsync(id);
    return oyun is null ? Results.NotFound() : Results.Ok(oyun);
});

app.MapPost("/api/oyunlar", async (OyunGirdi girdi, UygulamaDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(girdi.Ad))
        return Results.BadRequest("Ad bos olamaz.");

    var yeni = new Oyun
    {
        Ad = girdi.Ad,
        CikisYili = girdi.CikisYili,
        Bitirdim = girdi.Bitirdim
    };

    db.Oyunlar.Add(yeni);
    await db.SaveChangesAsync();

    return Results.Created($"/api/oyunlar/{yeni.Id}", yeni);
});

app.MapDelete("/api/oyunlar/{id:int}", async (int id, UygulamaDbContext db) =>
{
    var oyun = await db.Oyunlar.FindAsync(id);
    if (oyun is null) return Results.NotFound();

    db.Oyunlar.Remove(oyun);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();

record OyunGirdi(string Ad, int CikisYili, bool Bitirdim);