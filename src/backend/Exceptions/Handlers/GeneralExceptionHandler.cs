using Microsoft.AspNetCore.Diagnostics;

namespace Swallow.Exceptions.Handlers
{
    public class GeneralExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync("An error occurred while processing your request", cancellationToken: cancellationToken);
            return true;
        }
    }
}
