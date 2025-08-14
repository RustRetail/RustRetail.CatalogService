using Microsoft.AspNetCore.Http;

namespace RustRetail.CatalogService.Contracts.Products.CreateProduct
{
    public class CreateProductRequest
    {
        public ProductInfo Product { get; set; } = new ProductInfo();
        public IFormFileCollection? Images { get; set; } = null;
    }

    public class ProductInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
        public string SKU { get; set; } = string.Empty;
        public Guid? CategoryId { get; set; } = null;
        public Guid? BrandId { get; set; } = null;
    }
}
