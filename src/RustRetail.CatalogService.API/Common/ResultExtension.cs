using Microsoft.AspNetCore.Http.Features;
using RustRetail.SharedKernel.Domain.Abstractions;
using RustRetail.SharedKernel.Domain.Enums;
using System.Diagnostics;

namespace RustRetail.CatalogService.API.Common
{
    internal static class ResultExtension
    {
        internal static IResult HandleFailure(Result result, HttpContext httpContext) =>
            result switch
            {
                { IsSuccess: true } => throw new InvalidOperationException("Cannot handle failure for successful result!"),
                _ => result.ToProblemDetails(httpContext)
            };

        static IResult ToProblemDetails(this Result result, HttpContext httpContext)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException("Cannot create ProblemDetails from successful result");
            }
            if (result.Errors == null || result.Errors.Count() == 0)
            {
                throw new InvalidOperationException("Cannot create ProblemDetails from result with no error(s)");
            }

            var extensions = new Dictionary<string, object?>()
            {
                { "errors", result.Errors }
            };
            extensions.TryAdd("requestId", httpContext.TraceIdentifier);
            Activity? activity = httpContext.Features.Get<IHttpActivityFeature>()?.Activity;
            extensions.TryAdd("traceId", activity?.Id);

            // Since most of the time, result will only have 1 error
            // Request StatusCode/Title/Type will be select using the first error in the list
            return Results.Problem(
                statusCode: GetStatusCode(result.Errors[0].Type),
                title: GetTitle(result.Errors[0].Type),
                type: GetType(result.Errors[0].Type),
                detail: result.Errors[0].Description,
                instance: $"{httpContext.Request.Method} {httpContext.Request.Path}",
                extensions: extensions);
        }

        static int GetStatusCode(ErrorType errorType) =>
                errorType.Value switch
                {
                    2 => StatusCodes.Status400BadRequest,
                    3 => StatusCodes.Status404NotFound,
                    4 => StatusCodes.Status409Conflict,
                    5 => StatusCodes.Status401Unauthorized,
                    0 => 0,
                    6 => StatusCodes.Status403Forbidden,
                    _ => StatusCodes.Status500InternalServerError
                };

        static string GetTitle(ErrorType errorType) =>
            errorType.Value switch
            {
                2 => "Bad Request",
                3 => "Not Found",
                4 => "Conflict",
                5 => "Unauthorized",
                0 => "None",
                6 => "Forbidden",
                _ => "Server Failure"
            };

        static string GetType(ErrorType errorType) =>
            errorType.Value switch
            {
                2 => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                3 => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                4 => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                5 => "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
                0 => "None",
                6 => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3",
                _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };
    }
}
