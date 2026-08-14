using IlkApi.Dto;

namespace IlkApi.Servisler;

public interface IOyunServisi
{
    Task<List<OyunOku>> HepsiniGetirAsync();
    Task<OyunOku?> GetirAsync(int id);
    Task<OyunOku> EkleAsync(OyunYaz girdi);
    Task<bool> SilAsync(int id);
}