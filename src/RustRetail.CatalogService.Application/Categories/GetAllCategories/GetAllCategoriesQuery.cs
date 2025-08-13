using RustRetail.CatalogService.Contracts.Categories;
using RustRetail.SharedApplication.Abstractions;

namespace RustRetail.CatalogService.Application.Categories.GetAllCategories
{
    public record GetAllCategoriesQuery()
        : IQuery<List<CategorySummaryDto>>;
}
