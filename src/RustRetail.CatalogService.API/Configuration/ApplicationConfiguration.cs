using Asp.Versioning;
using Asp.Versioning.Builder;
using RustRetail.SharedInfrastructure.Logging.Serilog;
using RustRetail.SharedInfrastructure.MinimalApi;

namespace RustRetail.CatalogService.API.Configuration
{
    internal static class ApplicationConfiguration
    {
        internal static WebApplication ConfigureApplicationPipeline(this WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseExceptionHandler();
            app.UseMinimalApiEndpoints();
            app.UseSharedSerilogRequestLogging();

            return app;
        }

        private static WebApplication UseMinimalApiEndpoints(
            this WebApplication app)
        {
            // Configure Api versioning
            ApiVersionSet apiVersionSet = app.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1.0))
                .ReportApiVersions()
                .Build();
            RouteGroupBuilder versionedGroup = app
                .MapGroup("api/v{version:apiVersion}")
                .WithApiVersionSet(apiVersionSet);

            app.MapEndpoints(versionedGroup);

            return app;
        }
    }
}
