using FluentValidation;

namespace IlkApi.Dto.Dogrulayicilar;

public class OyunYazDogrulayici : AbstractValidator<OyunYaz>
{
    public OyunYazDogrulayici()
    {
        RuleFor(x => x.Ad)
            .NotEmpty().WithMessage("Ad bos olamaz.")
            .MaximumLength(200).WithMessage("Ad en fazla 200 karakter olabilir.");

        RuleFor(x => x.CikisYili)
            .InclusiveBetween(1950, DateTime.UtcNow.Year + 5)
            .WithMessage("Cikis yili gecersiz.");
    }
}