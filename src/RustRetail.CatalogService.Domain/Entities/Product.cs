using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RustRetail.CatalogService.Domain.Entities.Common;
using RustRetail.CatalogService.Domain.Events.Product;

namespace RustRetail.CatalogService.Domain.Entities
{
    public sealed class Product : MongoDbAggregateRoot<Guid>
    {
        private Product(Guid id) : base(id)
        {
        }

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
        public Guid? CategoryId { get; set; }

        [BsonElement("brand_id")]
        [BsonRepresentation(BsonType.String)]
        public Guid? BrandId { get; set; }

        public static Product Create(string name,
            string description,
            decimal price,
            string sku,
            Guid? categoryId = null,
            Guid? brandId = null,
            IFormFileCollection? images = null)
        {
            var product = new Product(Guid.NewGuid())
            {
                Name = name,
                Description = description,
                Price = price,
                SKU = sku,
                CategoryId = categoryId,
                BrandId = brandId
            };
            product.AddDomainEvent(new ProductCreatedDomainEvent(product.Id, name, images));
            return product;
        }
    }
}
