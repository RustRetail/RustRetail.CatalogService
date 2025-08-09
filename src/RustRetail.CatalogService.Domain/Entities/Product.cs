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
        public string CategoryId { get; set; } = string.Empty;

        [BsonElement("brand_id")]
        public string? BrandId { get; set; } = string.Empty;
    }
}
