using Microsoft.EntityFrameworkCore;
using IlkApi.Dto;
using IlkApi.Modeller;
using IlkApi.Veri;

namespace IlkApi.Servisler;

public class OyunServisi : IOyunServisi
{
    private readonly UygulamaDbContext _db;

    public OyunServisi(UygulamaDbContext db)
    {
        _db = db;
    }

    public async Task<List<OyunOku>> HepsiniGetirAsync()
    {
        return await _db.Oyunlar
            .Select(o => new OyunOku(o.Id, o.Ad, o.CikisYili, o.Bitirdim))
            .ToListAsync();
    }

    public async Task<OyunOku?> GetirAsync(int id)
    {
        var oyun = await _db.Oyunlar.FindAsync(id);
        return oyun is null
            ? null
            : new OyunOku(oyun.Id, oyun.Ad, oyun.CikisYili, oyun.Bitirdim);
    }

    public async Task<OyunOku> EkleAsync(OyunYaz girdi)
    {
        var yeni = new Oyun
        {
            Ad = girdi.Ad.Trim(),
            CikisYili = girdi.CikisYili,
            Bitirdim = girdi.Bitirdim
        };

        _db.Oyunlar.Add(yeni);
        await _db.SaveChangesAsync();

        return new OyunOku(yeni.Id, yeni.Ad, yeni.CikisYili, yeni.Bitirdim);
    }

    public async Task<bool> SilAsync(int id)
    {
        var oyun = await _db.Oyunlar.FindAsync(id);
        if (oyun is null) return false;

        _db.Oyunlar.Remove(oyun);
        await _db.SaveChangesAsync();
        return true;
    }
}