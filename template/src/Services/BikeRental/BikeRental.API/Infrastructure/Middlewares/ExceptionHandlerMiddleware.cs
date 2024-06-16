using Microsoft.AspNetCore.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;
using BikeRental.API.DTOs.V1.Responses;

namespace BikeRental.API.Infrastructure.Middlewares
{
    public static class ExceptionHandlerMiddleware
    {
        public static Action<IApplicationBuilder> ExceptionHandler(IWebHostEnvironment env)
        {
            return exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    context.Response.ContentType = Text.Plain;

                    await context.Response.WriteAsync("An exception was thrown.");

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        var json = JsonSerializer.Serialize(new ErrorResponseDto()
                        {
                            StatusCode = context.Response.StatusCode,
                            Messages = env.IsDevelopment() ? [contextFeature.Error.Message, contextFeature.Error.StackTrace ?? string.Empty] : ["Internal Server Error."]
                        });
                        await context.Response.WriteAsync(json);
                    }
                });
            };
        }
    }
}
