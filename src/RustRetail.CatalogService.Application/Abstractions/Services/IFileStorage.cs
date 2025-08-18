using Microsoft.AspNetCore.Http;

namespace RustRetail.CatalogService.Application.Abstractions.Services
{
    public record StorageObject(string Key, string Url, long? Length = null, string? ContentType = null, IDictionary<string, string>? Metadata = null);

    public interface IFileStorage
    {
        Task<StorageObject> UploadAsync(IFormFile file, string key = "", CancellationToken cancellationToken = default, IDictionary<string, string>? metadata = null);
        Task<IReadOnlyList<StorageObject>> UploadManyAsync(IFormFileCollection files, IReadOnlyList<string> keys, CancellationToken cancellationToken = default, IDictionary<string, string>? metadata = null);
        Task<StorageObject> UploadAsync(Stream content, string key, string contentType, CancellationToken cancellationToken = default, IDictionary<string, string>? metadata = null);
    }
}
