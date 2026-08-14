using FluentValidation;

namespace IlkApi.Dto.Dogrulayicilar;

// Giriste SADECE "deger var mi" kontrol edilir.
// Sifre politikasi (uzunluk, format) burada TEKRARLANMAZ: politika degistiginde
// eski sifreye sahip kullanicilar giris yapamaz hale gelirdi.
public class GirisIstekDogrulayici : AbstractValidator<GirisIstek>
{
    public GirisIstekDogrulayici()
    {
        RuleFor(x => x.Eposta)
            .NotEmpty().WithMessage("Eposta bos olamaz.");

        RuleFor(x => x.Sifre)
            .NotEmpty().WithMessage("Sifre bos olamaz.");
    }
}
