using MongoDB.Bson.Serialization.Attributes;
using RustRetail.SharedKernel.Domain.Events.Domain;

namespace RustRetail.CatalogService.Domain.Entities.Common
{
    public abstract class MongoDbAggregateRoot<TKey> : MongoDbEntity<TKey>, IHasDomainEvents
    {
        protected MongoDbAggregateRoot(TKey id) : base(id) { }

        [BsonIgnore]
        private readonly List<IDomainEvent> _domainEvents = new();

        [BsonIgnore]
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            if (domainEvent is null)
                throw new ArgumentNullException(nameof(domainEvent));
            _domainEvents.Add(domainEvent);
        }
    }
}
