using MongoDB.Driver;
using RustRetail.CatalogService.Contracts.Categories;
using RustRetail.CatalogService.Domain.Abstractions.Database;
using RustRetail.CatalogService.Domain.Entities;
using RustRetail.SharedApplication.Abstractions;
using RustRetail.SharedKernel.Domain.Abstractions;

namespace RustRetail.CatalogService.Application.Categories.GetAllCategories
{
    internal class GetAllCategoriesQueryHandler(
        ICatalogDbContext dbContext)
        : IQueryHandler<GetAllCategoriesQuery, List<CategorySummaryDto>>
    {
        public async Task<Result<List<CategorySummaryDto>>> Handle(
            GetAllCategoriesQuery request,
            CancellationToken cancellationToken)
        {
            var filter = Builders<Category>.Filter.Empty;
            var projection = Builders<Category>.Projection
                .Expression(p => new CategorySummaryDto(p.Id, p.Name));
            var categories = await dbContext.Categories.Find(filter)
                .Project(projection)
                .ToListAsync(cancellationToken);
            return Result.Success(categories);
        }
    }
}
