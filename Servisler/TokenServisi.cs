using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using IlkApi.Modeller;

namespace IlkApi.Servisler;

public class TokenServisi : ITokenServisi
{
    private readonly IConfiguration _yapilandirma;

    public TokenServisi(IConfiguration yapilandirma)
    {
        _yapilandirma = yapilandirma;
    }

    public (string Token, DateTime GecerlilikSonu) Uret(Kullanici kullanici)
    {
        var anahtar = _yapilandirma["Jwt:Anahtar"]
            ?? throw new InvalidOperationException("Jwt:Anahtar tanimli degil.");

        var gecerlilikSonu = DateTime.UtcNow.AddHours(1);

        var talepler = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, kullanici.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, kullanici.Eposta),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var imzalamaAnahtari = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(anahtar));
        var kimlikBilgisi = new SigningCredentials(imzalamaAnahtari, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _yapilandirma["Jwt:Yayinci"],
            audience: _yapilandirma["Jwt:Hedef"],
            claims: talepler,
            expires: gecerlilikSonu,
            signingCredentials: kimlikBilgisi);

        return (new JwtSecurityTokenHandler().WriteToken(token), gecerlilikSonu);
    }
}