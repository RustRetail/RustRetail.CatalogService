using Microsoft.AspNetCore.Http;

namespace RustRetail.CatalogService.Application.Common.Utilities
{
    public static class ImageChecker
    {
        // Temporary use hard coded values for now. Might change later.

        // Only allow png and jpg/jpeg extensions at the moment
        public static readonly string[] AllowedExtensions = { ".png", ".jpg", ".jpeg" };
        // Only allow png and jpg/jpeg content types. jpg/jpeg use the same "image/jpeg" Mime type.
        public static readonly string[] AllowedMimeTypes = { "image/png", "image/jpeg" };
        // Maximum file size of 5 MB
        public const long MaxFileSize = 5 * 1024 * 1024;

        public static bool IsImageSizeAllowed(IFormFile image)
            => image.Length <= MaxFileSize;

        public static bool IsImageExtensionAllowed(IFormFile image)
            => AllowedExtensions.Contains(Path.GetExtension(image.FileName).ToLower());

        public static bool IsImageMimeTypeAllowed(IFormFile image)
            => AllowedMimeTypes.Contains(image.ContentType.ToLower());
    }
}
