using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RustRetail.CatalogService.Domain.Entities.Common;

namespace RustRetail.CatalogService.Domain.Entities
{
    public class Brand : MongoDbEntity
    {
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("website")]
        public string? Website { get; set; }
    }
}
