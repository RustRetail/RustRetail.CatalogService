using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RustRetail.CatalogService.Application.Common.AggregateRoot;
using RustRetail.CatalogService.Domain.Abstractions.Database;
using RustRetail.CatalogService.Domain.Entities;
using RustRetail.CatalogService.Domain.Errors.Product;
using RustRetail.SharedApplication.Abstractions;
using RustRetail.SharedKernel.Domain.Abstractions;
using RustRetail.SharedKernel.Domain.Events.Domain;

namespace RustRetail.CatalogService.Application.Products.CreateProduct
{
    internal class CreateProductCommandHandler(
        ICatalogDbContext dbContext,
        IDomainEventDispatcher domainEventDispatcher)
        : ICommandHandler<CreateProductCommand>
    {
        public async Task<Result> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            string processedSKU = request.SKU.Trim().ToUpperInvariant();
            var isSkuExists = await dbContext.Products.AsQueryable().AnyAsync(p => p.SKU == processedSKU, cancellationToken);
            if (isSkuExists)
            {
                return Result.Failure(CreateProductErrors.SKUExisted);
            }

            var product = Product.Create(request.Name.Trim(),
                request.Description.Trim(),
                request.Price,
                processedSKU,
                request.CategoryId,
                request.BrandId,
                request.Images);

            await dbContext.Products.InsertOneAsync(product, cancellationToken: cancellationToken);

            await product.PublishAndClearMultipleDomainEvents(domainEventDispatcher, cancellationToken);

            return Result.Success();
        }
    }
}
