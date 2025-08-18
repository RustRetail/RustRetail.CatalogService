using RustRetail.CatalogService.Contracts.Brands;
using RustRetail.CatalogService.Contracts.Categories;

namespace RustRetail.CatalogService.Contracts.Products.GetById
{
    public record GetProductByIdResponse(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        string SKU,
        List<ProductImageResponse> Images,
        CategorySummaryDto? Category,
        BrandSummaryDto? Brand);

    public record ProductImageResponse(
        string Url,
        string? AltText);
}
