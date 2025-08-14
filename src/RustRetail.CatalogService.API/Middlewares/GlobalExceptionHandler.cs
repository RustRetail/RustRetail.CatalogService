using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using RustRetail.CatalogService.Domain.Errors.Common;
using RustRetail.SharedKernel.Domain.Abstractions;

namespace RustRetail.CatalogService.API.Middlewares
{
    internal class GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IHostEnvironment environment) : IExceptionHandler
    {
        const string ErrorCode = "CatalogService.UnexpectedError";

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            logger.LogError(exception, "Unexpected error occurred: {Message}", exception.Message);

            ProblemDetails problemDetails;

            if (exception is BadHttpRequestException badHttpRequestEx)
            {
                problemDetails = CreateProblemDetails(httpContext,
                    environment,
                    exception,
                    StatusCodes.Status400BadRequest,
                    ValidationErrors.RequestBodyMissing,
                    "Bad Request",
                    "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1");
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            else
            {
                var error = Error.Failure(ErrorCode, $"Unexpected error occurred: {exception.Message}");
                problemDetails = CreateProblemDetails(httpContext,
                    environment,
                    exception,
                    StatusCodes.Status500InternalServerError,
                    error,
                    "Internal Server Error",
                    "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1");
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }

        private static ProblemDetails CreateProblemDetails(
            HttpContext httpContext,
            IHostEnvironment env,
            Exception exception,
            int statusCode,
            Error error,
            string title,
            string type)
        {
            var extension = new Dictionary<string, object?>
            {
                ["requestId"] = httpContext.TraceIdentifier,
                ["traceId"] = httpContext.Features.Get<IHttpActivityFeature>()?.Activity?.Id,
                ["errors"] = new[] { error }
            };

            return new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = env.IsDevelopment()
                    ? exception.Message
                    : "Unexpected error occurred when trying to process request",
                Extensions = extension,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            };
        }
    }
}
