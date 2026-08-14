using FluentValidation;

namespace IlkApi.Dto.Dogrulayicilar;

public class KayitIstekDogrulayici : AbstractValidator<KayitIstek>
{
    public KayitIstekDogrulayici()
    {
        RuleFor(x => x.Eposta)
            .NotEmpty().WithMessage("Eposta bos olamaz.")
            .EmailAddress().WithMessage("Gecerli bir eposta giriniz.");

        RuleFor(x => x.Sifre)
            .NotEmpty().WithMessage("Sifre bos olamaz.")
            .MinimumLength(8).WithMessage("Sifre en az 8 karakter olmali.");
    }
}