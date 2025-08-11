using RustRetail.SharedKernel.Domain.Abstractions;

namespace RustRetail.CatalogService.Domain.Errors.Common
{
    public static class ValidationErrors
    {
        public static Error RequestBodyMissing = Error.Validation("Request.MissingBody", "The request body is missing or empty. Please provide a valid JSON object.");
        public static Error InvalidRequest = Error.Validation("Request.Invalid", "The request is invalid. Ensure the request body is not empty and all required fields are properly set.");
    }
}
