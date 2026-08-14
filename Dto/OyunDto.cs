namespace IlkApi.Dto;

public record OyunOku(int Id, string Ad, int CikisYili, bool Bitirdim);

public record OyunYaz(string Ad, int CikisYili, bool Bitirdim);