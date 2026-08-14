using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using IlkApi.Modeller;
using IlkApi.Ortak;

namespace IlkApi.Servisler;

public class TokenServisi : ITokenServisi
{
    private readonly JwtAyarlari _ayarlar;

    public TokenServisi(IOptions<JwtAyarlari> ayarlar)
    {
        _ayarlar = ayarlar.Value;
    }

    public (string Token, DateTime GecerlilikSonu) Uret(Kullanici kullanici)
    {
        var gecerlilikSonu = DateTime.UtcNow.AddMinutes(_ayarlar.GecerlilikDakika);

        var talepler = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, kullanici.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, kullanici.Eposta),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var imzalamaAnahtari = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_ayarlar.Anahtar));
        var kimlikBilgisi = new SigningCredentials(imzalamaAnahtari, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _ayarlar.Yayinci,
            audience: _ayarlar.Hedef,
            claims: talepler,
            expires: gecerlilikSonu,
            signingCredentials: kimlikBilgisi);

        return (new JwtSecurityTokenHandler().WriteToken(token), gecerlilikSonu);
    }
}
