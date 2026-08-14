namespace IlkApi.Ortak;

public record HataYaniti(string Mesaj, Dictionary<string, string[]>? Detaylar = null);