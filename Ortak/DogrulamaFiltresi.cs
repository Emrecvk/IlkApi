using FluentValidation;

namespace IlkApi.Ortak;

public class DogrulamaFiltresi<T> : IEndpointFilter where T : class
{
    private readonly IValidator<T> _dogrulayici;

    public DogrulamaFiltresi(IValidator<T> dogrulayici)
    {
        _dogrulayici = dogrulayici;
    }

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var girdi = context.Arguments.OfType<T>().FirstOrDefault();

        if (girdi is null)
            return Results.BadRequest(new HataYaniti("Istek govdesi okunamadi."));

        var sonuc = await _dogrulayici.ValidateAsync(girdi);

        if (!sonuc.IsValid)
        {
            var detaylar = sonuc.Errors
                .GroupBy(h => h.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(h => h.ErrorMessage).ToArray());

            return Results.BadRequest(new HataYaniti("Dogrulama basarisiz.", detaylar));
        }

        return await next(context);
    }
}