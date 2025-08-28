using MongoDB.Driver;
using RustRetail.CatalogService.Contracts.Common.Paging;
using RustRetail.CatalogService.Contracts.Products;
using RustRetail.CatalogService.Contracts.Products.SearchProducts;
using RustRetail.CatalogService.Domain.Abstractions.Database;
using RustRetail.CatalogService.Domain.Entities;
using RustRetail.SharedApplication.Abstractions;
using RustRetail.SharedKernel.Domain.Abstractions;

namespace RustRetail.CatalogService.Application.Products.SearchProducts
{
    internal class SearchProductsQueryHandler(
        ICatalogDbContext dbContext)
        : IQueryHandler<SearchProductsQuery, SearchProductsResponse>
    {
        public async Task<Result<SearchProductsResponse>> Handle(
            SearchProductsQuery request,
            CancellationToken cancellationToken)
        {
            FilterDefinition<Product> finalFilter = GetProductsFilter(request);
            SortDefinition<Product> sort = GetProductSortFilter(request);

            var projection = Builders<Product>.Projection.Expression(
                p => new ProductSummaryWithImageDto(p.Id, p.Name, p.Price, p.Images.First().Url));
            var query = dbContext.Products
                .Find(finalFilter);
            var count = (int)await query.CountDocumentsAsync(cancellationToken);
            var products = await query
                .Sort(sort)
                .Skip((request.Page - 1) * request.PageSize)
                .Limit(request.PageSize)
                .Project(projection)
                .ToListAsync(cancellationToken);
            return Result.Success(new SearchProductsResponse(
                PagedList<ProductSummaryWithImageDto>.Create(products, request.Page, request.PageSize, count, false)));
        }

        private static FilterDefinition<Product> GetProductsFilter(SearchProductsQuery request)
        {
            var fb = Builders<Product>.Filter;
            var filters = new List<FilterDefinition<Product>>();
            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                filters.Add(fb.Text(request.Keyword));
            }
            if (request.CategoryId.HasValue)
            {
                filters.Add(fb.Eq(p => p.CategoryId, request.CategoryId.Value));
            }
            if (request.BrandId.HasValue)
            {
                filters.Add(fb.Eq(p => p.BrandId, request.BrandId.Value));
            }
            if (request.MinPrice.HasValue)
            {
                filters.Add(fb.Gte(p => p.Price, request.MinPrice.Value));
            }
            if (request.MaxPrice.HasValue)
            {
                filters.Add(fb.Lte(p => p.Price, request.MaxPrice.Value));
            }
            if (!string.IsNullOrWhiteSpace(request.SKU))
            {
                filters.Add(fb.Eq(p => p.SKU, request.SKU));
            }
            filters.Add(fb.Eq(p => p.IsActive, true));
            var finalFilter = filters.Any()
                ? fb.And(filters)
                : fb.Empty;
            return finalFilter;
        }

        private static SortDefinition<Product> GetProductSortFilter(SearchProductsQuery request)
        {
            var sortBuilder = Builders<Product>.Sort;
            SortDefinition<Product> sort = null;
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                var isAscending = string.Equals(request.SortOrder, "asc", StringComparison.OrdinalIgnoreCase);
                sort = request.SortBy.ToLower() switch
                {
                    "price" => isAscending ? sortBuilder.Ascending(p => p.Price) : sortBuilder.Descending(p => p.Price),
                    "name" => isAscending ? sortBuilder.Ascending(p => p.Name) : sortBuilder.Descending(p => p.Name),
                    "created_at" => isAscending ? sortBuilder.Ascending(p => p.CreatedDateTime) : sortBuilder.Descending(p => p.CreatedDateTime),
                    _ => sortBuilder.Descending(p => p.CreatedDateTime)
                };
            }
            else
            {
                sort = sortBuilder.Descending(p => p.CreatedDateTime);
            }
            return sort;
        }
    }
}
