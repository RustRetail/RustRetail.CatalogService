namespace RustRetail.CatalogService.Contracts.Products
{
    public record ProductSummaryWithImageDto(
        Guid Id,
        string Name,
        decimal Price,
        string imageUrl);
}
