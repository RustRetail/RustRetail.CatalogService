using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RustRetail.CatalogService.Domain.Entities.Common;

namespace RustRetail.CatalogService.Domain.Entities
{
    public class Category : MongoDbEntity
    {
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("description")]
        public string? Description { get; set; }

        // For subcategories
        [BsonElement("parent_category_id")]
        [BsonRepresentation(BsonType.String)]
        public Guid? ParentCategoryId { get; set; } 
    }
}
