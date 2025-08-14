using Microsoft.AspNetCore.Http;
using RustRetail.SharedApplication.Abstractions;

namespace RustRetail.CatalogService.Application.Products.CreateProduct
{
    public record CreateProductCommand(
        string Name,
        string Description,
        decimal Price,
        string SKU,
        Guid? CategoryId,
        Guid? BrandId,
        IFormFileCollection? Images)
        : ICommand;
}
