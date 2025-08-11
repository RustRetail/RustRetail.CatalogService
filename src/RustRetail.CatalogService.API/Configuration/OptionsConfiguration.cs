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

            return services;
        }
    }
}
