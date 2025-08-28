using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RustRetail.SharedApplication.Behaviors.Messaging;

namespace RustRetail.CatalogService.Infrastructure.MessageBrokers.RabbitMQ
{
    internal static class RabbitMQServiceCollectionExtensions
    {
        internal static IServiceCollection AddRabbitMQ(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.UsingRabbitMq((context, cfg) =>
                {
                    var options = configuration.GetSection(RabbitMQOptions.SectionName).Get<RabbitMQOptions>();
                    ArgumentNullException.ThrowIfNull(options, nameof(options));
                    cfg.Host(options.Host, (ushort)options.Port, options.VirtualHost, h =>
                    {
                        h.Username(options.UserName);
                        h.Password(options.Password);
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });
            services.AddScoped<IMessageBus, MassTransitMessageSender>();
            return services;
        }
    }
}
