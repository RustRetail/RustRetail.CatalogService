namespace RustRetail.CatalogService.Infrastructure.Storage.S3
{
    public class AwsS3Options
    {
        public const string SectionName = "AwsS3";

        public string Profile { get; set; } = default!;
        public string BucketName { get; set; } = default!;
        public string Region { get; set; } = default!;
        public string AccessKey { get; set; } = default!;
        public string SecretKey { get; set; } = default!;
        public string PublicBaseUrl { get; set; } = default!;
        public string DefaultCacheControl { get; set; } = default!;
    }
}
