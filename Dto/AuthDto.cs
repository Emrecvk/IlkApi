namespace IlkApi.Dto;

public record KayitIstek(string Eposta, string Sifre);
public record GirisIstek(string Eposta, string Sifre);
public record TokenYanit(string Token, DateTime GecerlilikSonu);