namespace RustRetail.CatalogService.Application.Common.Utilities
{
    public static class S3ImageKeyBuilder
    {
        public static string BuildProductImageKey(string productId, string fileName)
        {
            return $"public/products/{productId}/images/{fileName}";
        }
    }
}
