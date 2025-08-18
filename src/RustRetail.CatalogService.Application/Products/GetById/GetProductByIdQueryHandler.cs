using MongoDB.Driver;
using RustRetail.CatalogService.Contracts.Products.GetById;
using RustRetail.CatalogService.Domain.Abstractions.Database;
using RustRetail.CatalogService.Domain.Errors.Product;
using RustRetail.SharedApplication.Abstractions;
using RustRetail.SharedKernel.Domain.Abstractions;

namespace RustRetail.CatalogService.Application.Products.GetById
{
    internal class GetProductByIdQueryHandler(
        ICatalogDbContext dbContext)
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResponse>
    {
        public async Task<Result<GetProductByIdResponse>> Handle(
            GetProductByIdQuery request,
            CancellationToken cancellationToken)
        {
            var product = await dbContext.Products.Find(p => p.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                return Result.Failure<GetProductByIdResponse>(ProductErrors.ProductNotFound);
            }

            var category = await dbContext.Categories.Find(c => c.Id == product.CategoryId)
                .FirstOrDefaultAsync(cancellationToken);
            var brand = await dbContext.Brands.Find(b => b.Id == product.BrandId)
                .FirstOrDefaultAsync(cancellationToken);

            var response = new GetProductByIdResponse(product.Id, product.Name, product.Description, product.Price, product.SKU,
                product.Images.Select(i => new ProductImageResponse(i.Url, i.AltText)).ToList(),
                category is null ? null : new Contracts.Categories.CategorySummaryDto(category.Id, category.Name),
                brand is null ? null : new Contracts.Brands.BrandSummaryDto(brand.Id, brand.Name));

            return Result.Success(response);
        }
    }
}
