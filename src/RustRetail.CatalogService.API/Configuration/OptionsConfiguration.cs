using RustRetail.CatalogService.Infrastructure.Storage.S3;
using RustRetail.CatalogService.Persistence.Database;

namespace RustRetail.CatalogService.API.Configuration
{
    internal static class OptionsConfiguration
    {
        internal static IServiceCollection ConfigureApplicationOptions(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Persistence
            services.Configure<MongoDbSettings>(configuration.GetSection(MongoDbSettings.SectionName));

            // Infrastructure
            services.Configure<AwsS3Options>(configuration.GetSection(AwsS3Options.SectionName));

            return services;
        }
    }
}
