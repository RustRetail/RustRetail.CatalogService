using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RustRetail.CatalogService.Domain.Abstractions.Database;
using RustRetail.CatalogService.Persistence.Database;

namespace RustRetail.CatalogService.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services)
        {
            return services
                .RegisterMongoDbClient()
                .AddSingleton<ICatalogDbContext, CatalogDbContext>(); ;
        }

        private static IServiceCollection RegisterMongoDbClient(this IServiceCollection services)
        {
            return services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                if (string.IsNullOrWhiteSpace(settings.ConnectionString))
                {
                    throw new InvalidOperationException("MongoDbSettings:ConnectionString is not configured.");
                }
                return new MongoClient(settings.ConnectionString);
            });
        }
    }
}
