using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RustRetail.CatalogService.Infrastructure.MessageBrokers.RabbitMQ;
using RustRetail.CatalogService.Infrastructure.Storage;

namespace RustRetail.CatalogService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.AddStorageServices(configuration)
                .AddRabbitMQ(configuration);
        }
    }
}
