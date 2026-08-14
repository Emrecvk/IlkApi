using IlkApi.Dto;
using IlkApi.Ortak;
using IlkApi.Servisler;

namespace IlkApi.Endpointler;

public static class AuthEndpointleri
{
    public static void AuthEndpointleriniEkle(this WebApplication app)
    {
        var grup = app.MapGroup("/api/auth");

        grup.MapPost("/kayit", async (KayitIstek istek, IAuthServisi servis) =>
        {
            var sonuc = await servis.KayitOlAsync(istek);
            return sonuc is null
                ? Results.Conflict(new HataYaniti("Bu eposta zaten kayitli."))
                : Results.Ok(sonuc);
        })
        .AddEndpointFilter<DogrulamaFiltresi<KayitIstek>>();

        grup.MapPost("/giris", async (GirisIstek istek, IAuthServisi servis) =>
        {
            var sonuc = await servis.GirisYapAsync(istek);
            return sonuc is null
                ? Results.Unauthorized()
                : Results.Ok(sonuc);
        });
    }
}