using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RustRetail.CatalogService.Domain.Entities.Common;

namespace RustRetail.CatalogService.Domain.Entities
{
    public class Product : MongoDbEntity
    {
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("sku")]
        public string SKU { get; set; } = string.Empty;

        [BsonElement("images")]
        public List<ProductImage> Images { get; set; } = new();

        [BsonElement("category_id")]
        [BsonRepresentation(BsonType.String)]
        public Guid CategoryId { get; set; }

        [BsonElement("brand_id")]
        [BsonRepresentation(BsonType.String)]
        public Guid? BrandId { get; set; }
    }
}
