using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Swallow.Services;
using System.Net;

namespace Swallow.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature?.Error;

                    if (exception != null)
                    {
                        var errorLogService = context.RequestServices.GetRequiredService<IErrorLogger>();
                        await errorLogService.LogError(exception);

                        var result = JsonConvert.SerializeObject(new { error = "Internal Server Error" });
                        await context.Response.WriteAsync(result);
                    }
                });
            });
        }

        public static void AddCustomMiddleware(this IApplicationBuilder app)
        {
            app.UseCustomExceptionHandler();
        }
    }
}
