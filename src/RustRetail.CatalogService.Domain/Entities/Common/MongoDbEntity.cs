using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RustRetail.SharedKernel.Domain.Abstractions;

namespace RustRetail.CatalogService.Domain.Entities.Common
{
    public abstract class MongoDbEntity : IHasKey<Guid>, ITrackable
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("created_at")]
        public DateTimeOffset? CreatedDateTime { get; set; } = DateTimeOffset.UtcNow;

        [BsonElement("updated_at")]
        public DateTimeOffset? UpdatedDateTime { get; set; } = null;

        public void SetCreatedDateTime(DateTimeOffset? createdDateTime)
        {
            CreatedDateTime = createdDateTime;
        }

        public void SetUpdatedDateTime(DateTimeOffset? updatedDateTime)
        {
            UpdatedDateTime = updatedDateTime;
        }
    }
}
