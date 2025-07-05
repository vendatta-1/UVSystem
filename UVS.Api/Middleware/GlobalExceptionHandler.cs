using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace UVS.Api.Middleware;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    :IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, exception.Message);
        var problem = new ProblemDetails()
        {
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception.Message,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Title = "Internal Server Error"
        };
        
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        
        return true;
    }
}