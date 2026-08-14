using IlkApi.Dto;

namespace IlkApi.Servisler;

public interface IAuthServisi
{
    Task<TokenYanit?> KayitOlAsync(KayitIstek istek);
    Task<TokenYanit?> GirisYapAsync(GirisIstek istek);
}