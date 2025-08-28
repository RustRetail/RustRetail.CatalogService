using MassTransit;
using RustRetail.SharedApplication.Behaviors.Messaging;

namespace RustRetail.CatalogService.Infrastructure.MessageBrokers.RabbitMQ
{
    internal class MassTransitMessageSender : IMessageBus
    {
        readonly IPublishEndpoint _publishEndpoint;

        public MassTransitMessageSender(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }
        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
        {
            ArgumentNullException.ThrowIfNull(message);
            await _publishEndpoint.Publish(message, cancellationToken);
        }

        public async Task PublishAsync(object message, Type type, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(type);
            await _publishEndpoint.Publish(message, type, cancellationToken);
        }
    }
}
