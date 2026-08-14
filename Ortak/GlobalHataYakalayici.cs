using Microsoft.AspNetCore.Diagnostics;

namespace IlkApi.Ortak;

public class GlobalHataYakalayici : IExceptionHandler
{
    private readonly ILogger<GlobalHataYakalayici> _log;

    public GlobalHataYakalayici(ILogger<GlobalHataYakalayici> log)
    {
        _log = log;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _log.LogError(exception, "Yakalanmayan hata: {Yol}", httpContext.Request.Path);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(
            new HataYaniti("Beklenmeyen bir hata olustu."),
            cancellationToken);

        return true;
    }
}