using IlkApi.Dto;
using IlkApi.Servisler;

namespace IlkApi.Endpointler;

public static class OyunEndpointleri
{
    public static void OyunEndpointleriniEkle(this WebApplication app)
    {
        var grup = app.MapGroup("/api/oyunlar");

        grup.MapGet("/", async (IOyunServisi servis) =>
            Results.Ok(await servis.HepsiniGetirAsync()));

        grup.MapGet("/{id:int}", async (int id, IOyunServisi servis) =>
        {
            var oyun = await servis.GetirAsync(id);
            return oyun is null ? Results.NotFound() : Results.Ok(oyun);
        });

        grup.MapPost("/", async (OyunYaz girdi, IOyunServisi servis) =>
        {
            if (string.IsNullOrWhiteSpace(girdi.Ad))
                return Results.BadRequest("Ad bos olamaz.");

            var yeni = await servis.EkleAsync(girdi);
            return Results.Created($"/api/oyunlar/{yeni.Id}", yeni);
        });

        grup.MapDelete("/{id:int}", async (int id, IOyunServisi servis) =>
        {
            var silindi = await servis.SilAsync(id);
            return silindi ? Results.NoContent() : Results.NotFound();
        });
    }
}