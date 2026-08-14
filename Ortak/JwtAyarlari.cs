using System.ComponentModel.DataAnnotations;

namespace IlkApi.Ortak;

public class JwtAyarlari
{
    public const string Bolum = "Jwt";

    [Required(ErrorMessage = "Jwt:Anahtar tanimli degil.")]
    [MinLength(32, ErrorMessage = "Jwt:Anahtar en az 32 karakter olmali (HMAC-SHA256 icin 256 bit).")]
    public string Anahtar { get; set; } = string.Empty;

    [Required(ErrorMessage = "Jwt:Yayinci tanimli degil.")]
    public string Yayinci { get; set; } = string.Empty;

    [Required(ErrorMessage = "Jwt:Hedef tanimli degil.")]
    public string Hedef { get; set; } = string.Empty;

    [Range(1, 1440, ErrorMessage = "Jwt:GecerlilikDakika 1 ile 1440 arasinda olmali.")]
    public int GecerlilikDakika { get; set; } = 60;
}
