using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using RustRetail.SharedKernel.Domain.Abstractions;
using System.Diagnostics;

namespace RustRetail.CatalogService.API.Middlewares
{
    internal class GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IHostEnvironment environment) : IExceptionHandler
    {
        const string ErrorCode = "CatalogService.GlobalExceptionHandler.UnexpectedError";

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            logger.LogError(exception, "Unexpected error occurred: {Message}", exception.Message);

            var extension = new Dictionary<string, object?>();
            extension.TryAdd("requestId", httpContext.TraceIdentifier);
            Activity? activity = httpContext.Features.Get<IHttpActivityFeature>()?.Activity;
            extension.TryAdd("traceId", activity?.Id);
            var error = Error.Failure(ErrorCode, $"Unexpected error occurred: {exception.Message}");
            extension.TryAdd("errors", new[] { error });

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                Detail = environment.IsDevelopment() ? exception.Message : "Unexpected error occurred when trying to process request",
                Extensions = extension,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
            };

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
