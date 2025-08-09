using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RustRetail.CatalogService.Domain.Abstractions;
using RustRetail.SharedKernel.Domain.Abstractions;

namespace RustRetail.CatalogService.Domain.Entities.Common
{
    public abstract class MongoDbEntity : IHasKey<Guid>, IMongoDbTrackable
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("created_at")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? CreatedDateTime { get; set; } = DateTime.UtcNow;

        [BsonElement("updated_at")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? UpdatedDateTime { get; set; } = null;

        public void SetCreatedDateTime(DateTime? createdDateTime)
        {
            CreatedDateTime = createdDateTime;
        }

        public void SetUpdatedDateTime(DateTime? updatedDateTime)
        {
            UpdatedDateTime = updatedDateTime;
        }
    }
}
