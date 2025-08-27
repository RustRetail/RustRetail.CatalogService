using RustRetail.CatalogService.Contracts.Common.Paging;

namespace RustRetail.CatalogService.Contracts.Products.SearchProducts
{
    public record SearchProductsResponse(
        PagedList<ProductSummaryWithImageDto> Products);
}
