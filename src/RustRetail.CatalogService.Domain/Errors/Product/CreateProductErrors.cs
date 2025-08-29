using RustRetail.SharedKernel.Domain.Abstractions;

namespace RustRetail.CatalogService.Domain.Errors.Product
{
    public static class CreateProductErrors
    {
        public static readonly Error SKUExisted = Error.Conflict("Product.Create.SKUExisted", "A product with the same SKU has existed.");
    }
}
