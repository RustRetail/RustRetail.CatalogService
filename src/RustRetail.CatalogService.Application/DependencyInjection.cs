using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RustRetail.SharedApplication.Behaviors.Event;
using RustRetail.SharedApplication.Behaviors.Logging;
using RustRetail.SharedKernel.Domain.Events.Domain;

namespace RustRetail.CatalogService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.ConfigureMediatR()
                .AddRequestLoggingBehavior()
                .AddDomainEventDispatcher();

            return services;
        }

        private static IServiceCollection ConfigureMediatR(
            this IServiceCollection services)
        {
            return services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });
        }

        private static IServiceCollection AddRequestLoggingBehavior(
            this IServiceCollection services)
        {
            return services.AddScoped(
                typeof(IPipelineBehavior<,>),
                typeof(LoggingBehavior<,>));
        }

        private static IServiceCollection AddDomainEventDispatcher(
            this IServiceCollection services)
        {
            return services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
        }
    }
}
