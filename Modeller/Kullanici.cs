namespace IlkApi.Modeller;

public class Kullanici
{
    public int Id { get; set; }
    public string Eposta { get; set; } = string.Empty;
    public string SifreHash { get; set; } = string.Empty;
    public DateTime OlusturulmaTarihi { get; set; } = DateTime.UtcNow;
}