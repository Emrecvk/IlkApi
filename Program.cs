var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var oyunlar = new List<Oyun>
{
    new Oyun(1, "Hollow Knight", 2017, true),
    new Oyun(2, "Vampire Survivors", 2022, true),
    new Oyun(3, "Megabonk", 2025, false)
};

var sonrakiId = 4;

// Hepsini getir
app.MapGet("/api/oyunlar", () => oyunlar);

// Tek kayıt getir
app.MapGet("/api/oyunlar/{id:int}", (int id) =>
{
    var oyun = oyunlar.FirstOrDefault(o => o.Id == id);
    return oyun is null
        ? Results.NotFound()
        : Results.Ok(oyun);
});

// Yeni kayıt ekle
app.MapPost("/api/oyunlar", (OyunGirdi girdi) =>
{
    if (string.IsNullOrWhiteSpace(girdi.Ad))
        return Results.BadRequest("Ad bos olamaz.");

    var yeni = new Oyun(sonrakiId++, girdi.Ad, girdi.CikisYili, girdi.Bitirdim);
    oyunlar.Add(yeni);

    return Results.Created($"/api/oyunlar/{yeni.Id}", yeni);
});

// Sil
app.MapDelete("/api/oyunlar/{id:int}", (int id) =>
{
    var oyun = oyunlar.FirstOrDefault(o => o.Id == id);
    if (oyun is null)
        return Results.NotFound();

    oyunlar.Remove(oyun);
    return Results.NoContent();
});

app.Run();

record Oyun(int Id, string Ad, int CikisYili, bool Bitirdim);
record OyunGirdi(string Ad, int CikisYili, bool Bitirdim);