using Microsoft.EntityFrameworkCore;
using IlkApi.Dto;
using IlkApi.Modeller;
using IlkApi.Veri;

namespace IlkApi.Servisler;

public class AuthServisi : IAuthServisi
{
    private readonly UygulamaDbContext _db;
    private readonly ITokenServisi _tokenServisi;

    public AuthServisi(UygulamaDbContext db, ITokenServisi tokenServisi)
    {
        _db = db;
        _tokenServisi = tokenServisi;
    }

    public async Task<TokenYanit?> KayitOlAsync(KayitIstek istek)
    {
        var eposta = istek.Eposta.Trim().ToLowerInvariant();

        var mevcut = await _db.Kullanicilar.AnyAsync(k => k.Eposta == eposta);
        if (mevcut) return null;

        var kullanici = new Kullanici
        {
            Eposta = eposta,
            SifreHash = BCrypt.Net.BCrypt.HashPassword(istek.Sifre)
        };

        _db.Kullanicilar.Add(kullanici);
        await _db.SaveChangesAsync();

        var (token, sona) = _tokenServisi.Uret(kullanici);
        return new TokenYanit(token, sona);
    }

    public async Task<TokenYanit?> GirisYapAsync(GirisIstek istek)
    {
        var eposta = istek.Eposta.Trim().ToLowerInvariant();

        var kullanici = await _db.Kullanicilar
            .FirstOrDefaultAsync(k => k.Eposta == eposta);

        if (kullanici is null) return null;

        var dogruMu = BCrypt.Net.BCrypt.Verify(istek.Sifre, kullanici.SifreHash);
        if (!dogruMu) return null;

        var (token, sona) = _tokenServisi.Uret(kullanici);
        return new TokenYanit(token, sona);
    }
}