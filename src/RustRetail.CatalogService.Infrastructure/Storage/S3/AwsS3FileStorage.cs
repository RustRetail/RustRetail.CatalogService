using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RustRetail.CatalogService.Application.Abstractions.Services;
using System.Net;

namespace RustRetail.CatalogService.Infrastructure.Storage.S3
{
    internal class AwsS3FileStorage : IFileStorage
    {
        const string DefaultFileContentType = "application/octet-stream";

        readonly IAmazonS3 _s3;
        readonly AwsS3Options _options;
        readonly ILogger<AwsS3FileStorage> _logger;

        public AwsS3FileStorage(IAmazonS3 s3, IOptions<AwsS3Options> options, ILogger<AwsS3FileStorage> logger)
        {
            _s3 = s3;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<StorageObject> UploadAsync(IFormFile file, string key = "", CancellationToken cancellationToken = default, IDictionary<string, string>? metadata = null)
        {
            if (file is null || file.Length == 0)
            {
                throw new ArgumentException("File is required and cannot be empty", nameof(file));
            }
            if (string.IsNullOrWhiteSpace(key)) key = file.FileName;
            await using var stream = file.OpenReadStream();
            return await UploadAsync(stream, key, file.ContentType, cancellationToken, metadata);
        }

        public async Task<StorageObject> UploadAsync(Stream content, string key, string contentType, CancellationToken cancellationToken = default, IDictionary<string, string>? metadata = null)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException(nameof(key));
            if (string.IsNullOrWhiteSpace(contentType)) contentType = DefaultFileContentType;
            var request = new PutObjectRequest
            {
                BucketName = _options.BucketName,
                InputStream = content,
                ContentType = contentType,
                Key = key,
                Headers = { CacheControl = _options.DefaultCacheControl }
            };
            AssignPutRequestMetadata(request, metadata);
            try
            {
                var response = await _s3.PutObjectAsync(request, cancellationToken);
                if (response.HttpStatusCode is not HttpStatusCode.OK)
                {
                    throw new InvalidOperationException($"S3 PutObject failed with status {response.HttpStatusCode}");
                }
                return new StorageObject(key, GetPublicUrl(key));
            }
            catch (AmazonS3Exception ex)
            {
                _logger.LogError(ex, "Failed to upload to S3 (Bucket: {Bucket}, Key: {Key})", _options.BucketName, key);
                throw;
            }
        }

        public async Task<IReadOnlyList<StorageObject>> UploadManyAsync(IFormFileCollection files, IReadOnlyList<string> keys, CancellationToken cancellationToken = default, IDictionary<string, string>? metadata = null)
        {
            if (files == null || files.Count == 0)
                throw new ArgumentException("No files provided", nameof(files));
            if (keys == null || keys.Count != files.Count)
                throw new ArgumentException("Keys list must match files count", nameof(keys));

            var results = new List<StorageObject>(files.Count);
            for (int i = 0; i < files.Count; i++)
            {
                if (files[i] is null || files[i].Length == 0) continue;
                results.Add(await UploadAsync(files[i], keys[i], cancellationToken, metadata));
            }
            return results;
        }

        private void AssignPutRequestMetadata(PutObjectRequest request,
            IDictionary<string, string>? metadata)
        {
            if (metadata is null) return;
            foreach (var kv in metadata)
            {
                request.Metadata.Add(kv.Key, kv.Value);
            }
        }

        public string GetPublicUrl(string key)
        {
            if (!string.IsNullOrWhiteSpace(_options.PublicBaseUrl))
            {
                return $"{_options.PublicBaseUrl.TrimEnd('/')}/{key}";
            }
            // Default S3 virtual-hosted–style URL
            return $"https://{_options.BucketName}.s3.{_options.Region}.amazonaws.com/{key}";
        }
    }
}
