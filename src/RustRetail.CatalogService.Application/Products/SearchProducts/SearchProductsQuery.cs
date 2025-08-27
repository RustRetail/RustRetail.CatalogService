using RustRetail.CatalogService.Contracts.Products.SearchProducts;
using RustRetail.SharedApplication.Abstractions;

namespace RustRetail.CatalogService.Application.Products.SearchProducts
{
    public record SearchProductsQuery(
        string? Keyword,
        Guid? CategoryId,
        Guid? BrandId,
        decimal? MinPrice,
        decimal? MaxPrice,
        string? SKU,
        string? SortBy,
        string? SortOrder,
        int Page = 1,
        int PageSize = 10) : IQuery<SearchProductsResponse>;
}
