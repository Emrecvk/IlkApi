using IlkApi.Modeller;

namespace IlkApi.Servisler;

public interface ITokenServisi
{
    (string Token, DateTime GecerlilikSonu) Uret(Kullanici kullanici);
}