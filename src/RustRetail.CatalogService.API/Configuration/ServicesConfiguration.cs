using FluentValidation;
using Microsoft.AspNetCore.Http.Features;
using RustRetail.CatalogService.API.Configuration.ApiVersioning;
using RustRetail.CatalogService.API.Configuration.JsonSerialization;
using RustRetail.CatalogService.API.Middlewares;
using RustRetail.SharedInfrastructure.MinimalApi;
using System.Diagnostics;

namespace RustRetail.CatalogService.API.Configuration
{
    internal static class ServicesConfiguration
    {
        internal static IServiceCollection AddApi(this IServiceCollection services)
        {
            return services.AddGlobalExceptionHandling()
                .ConfigureApiVersioning()
                .AddMinimalApi()
                .AddValidatorsFromAssembly(typeof(ServicesConfiguration).Assembly)
                .AddJsonSerializationConfiguration();
        }

        internal static IServiceCollection AddBuiltInServices(
            this IServiceCollection services)
        {
            return services.AddOpenApi()
                .AddHttpContextAccessor();
        }

        private static IServiceCollection AddGlobalExceptionHandling(
            this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = (context) =>
                {
                    context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
                    Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                    context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
                };
            });

            return services;
        }

        private static IServiceCollection AddMinimalApi(this IServiceCollection services)
        {
            return services.AddEndpoints(typeof(ServicesConfiguration).Assembly);
        }
    }
}
