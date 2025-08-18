using RustRetail.CatalogService.Application.Common.AggregateRoot;
using RustRetail.CatalogService.Domain.Abstractions.Database;
using RustRetail.CatalogService.Domain.Entities;
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
            var product = Product.Create(request.Name,
                request.Description,
                request.Price,
                request.SKU,
                request.CategoryId,
                request.BrandId,
                request.Images);

            await dbContext.Products.InsertOneAsync(product, cancellationToken: cancellationToken);

            await product.PublishAndClearMultipleDomainEvents(domainEventDispatcher, cancellationToken);

            return Result.Success();
        }
    }
}
