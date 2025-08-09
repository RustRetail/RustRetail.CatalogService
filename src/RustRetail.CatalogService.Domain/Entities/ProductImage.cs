using MongoDB.Bson.Serialization.Attributes;

namespace RustRetail.CatalogService.Domain.Entities
{
    public class ProductImage
    {
        [BsonElement("url")]
        public string Url { get; set; } = string.Empty;

        [BsonElement("alt_text")]
        public string? AltText { get; set; } = null;

        public static ProductImage Create(string url, string? altText = null)
        {
            return new ProductImage()
            {
                Url = url,
                AltText = altText,
            };
        }
    }
}
