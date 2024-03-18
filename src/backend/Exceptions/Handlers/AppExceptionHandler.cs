using Microsoft.AspNetCore.Diagnostics;
using Swallow.Exceptions.CustomExceptions;

namespace Swallow.Exceptions.Handlers
{
    public class AppExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            (int statusCode, string errorMessage) = exception switch
            {
                ForbidException => (StatusCodes.Status403Forbidden, "Forbidden"),
                BadRequestException badRequestException => (StatusCodes.Status400BadRequest, badRequestException.Message),
                NotFoundException notFoundException => (StatusCodes.Status404NotFound, notFoundException.Message),
                _ => default
            };

            if (statusCode == default)
            {
                return false;
            }

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(errorMessage, cancellationToken: cancellationToken);

            return true;
        }
    }
}
