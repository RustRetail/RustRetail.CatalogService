using MongoDB.Bson.Serialization.Attributes;

namespace RustRetail.CatalogService.Domain.Entities
{
    public sealed class ProductImage
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

        public static string GenerateImageUrl(string productId, string imageName)
        {
            return $"{productId}/images/{imageName}";
        }

        public static string GenerateImageAltText(string productName, string imageName)
        {
            return $"Image {imageName} of product {productName}.";
        }
    }
}
