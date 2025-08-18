using RustRetail.SharedKernel.Domain.Abstractions;

namespace RustRetail.CatalogService.Domain.Errors.Product
{
    public static class ProductErrors
    {
        public static readonly Error ProductNotFound = Error.NotFound("Product.NotFound", "The product was not found.");
    }
}
