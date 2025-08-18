using RustRetail.CatalogService.Domain.Entities.Common;
using RustRetail.SharedKernel.Domain.Events.Domain;

namespace RustRetail.CatalogService.Application.Common.AggregateRoot
{
    internal static class AggregateRootExtension
    {
        public static async Task PublishAndClearMultipleDomainEvents<TKey>(
            this MongoDbAggregateRoot<TKey> aggregateRoot,
            IDomainEventDispatcher domainEventDispatcher,
            CancellationToken cancellationToken = default)
        {
            await domainEventDispatcher.DispatchMultipleAsync(aggregateRoot.DomainEvents, cancellationToken);
            aggregateRoot.ClearDomainEvents();
        }
    }
}
