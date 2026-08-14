using Microsoft.EntityFrameworkCore;
using Npgsql;
using IlkApi.Dto;
using IlkApi.Modeller;
using IlkApi.Veri;

namespace IlkApi.Servisler;

public class AuthServisi : IAuthServisi
{
    // Kullanici bulunamadiginda da BCrypt calistirilir ki yanit suresi degismesin.
    // Ayni maliyet katsayisiyla uretilmis olmasi sart, aksi halde fark yine olculur.
    private static readonly string SahteHash =
        BCrypt.Net.BCrypt.HashPassword("zamanlama-farkini-kapatan-yer-tutucu");

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

        try
        {
            await _db.SaveChangesAsync();
        }
        // Yukaridaki kontrol ile bu satir arasinda baska bir istek ayni epostayi
        // eklemis olabilir. Unique index veriyi korur; burada da dogru cevaba cevrilir.
        // Sadece unique ihlali yakalanir: diger veritabani hatalari gizlenmemeli.
        catch (DbUpdateException hata)
            when (hata.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            return null;
        }

        var (token, sona) = _tokenServisi.Uret(kullanici);
        return new TokenYanit(token, sona);
    }

    public async Task<TokenYanit?> GirisYapAsync(GirisIstek istek)
    {
        var eposta = istek.Eposta.Trim().ToLowerInvariant();

        var kullanici = await _db.Kullanicilar
            .FirstOrDefaultAsync(k => k.Eposta == eposta);

        // Kullanici yoksa bile dogrulama yapilir; erken donus zamanlama sizintisi olurdu.
        var dogruMu = BCrypt.Net.BCrypt.Verify(istek.Sifre, kullanici?.SifreHash ?? SahteHash);

        if (kullanici is null || !dogruMu) return null;

        var (token, sona) = _tokenServisi.Uret(kullanici);
        return new TokenYanit(token, sona);
    }
}
