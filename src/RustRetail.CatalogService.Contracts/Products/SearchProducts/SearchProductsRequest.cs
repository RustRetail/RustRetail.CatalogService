namespace RustRetail.CatalogService.Contracts.Products.SearchProducts
{
    public record SearchProductsRequest(
        string? Keyword,
        Guid? CategoryId,
        Guid? BrandId,
        decimal? MinPrice,
        decimal? MaxPrice,
        string? SKU,
        string? SortBy,
        string? SortOrder,
        int Page = 1,
        int PageSize = 10);
}
