using FluentValidation;
using MediatR;
using RustRetail.CatalogService.API.Common;
using RustRetail.CatalogService.Application.Products.SearchProducts;
using RustRetail.CatalogService.Contracts.Products.SearchProducts;
using RustRetail.SharedInfrastructure.MinimalApi;

namespace RustRetail.CatalogService.API.Endpoints.V1.Products
{
    public class SearchProducts : IEndpoint
    {
        const string Route = $"{Resources.Products}";

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet(Route, Handle)
                .WithTags(Tags.Products)
                .AllowAnonymous()
                .MapToApiVersion(1)
                .AddEndpointFilter<ValidationFilter<SearchProductsRequest>>();
        }

        static async Task<IResult> Handle(
            [AsParameters] SearchProductsRequest request,
            HttpContext httpContext,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var query = new SearchProductsQuery(request.Keyword, request.CategoryId, request.BrandId,
                request.MinPrice, request.MaxPrice, request.SKU,
                request.SortBy, request.SortOrder, request.Page, request.PageSize);
            var result = await sender.Send(query, cancellationToken);
            return result.IsSuccess
                ? Results.Ok(new SuccessResultWrapper<SearchProductsResponse>(result, httpContext))
                : ResultExtension.HandleFailure(result, httpContext);
        }
    }

    public class SearchProductsRequestValidator : AbstractValidator<SearchProductsRequest>
    {
        public SearchProductsRequestValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(r => r.Page)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");
            RuleFor(r => r.PageSize)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(50)
                .WithMessage("Page size cannot exceed 50.");
            RuleFor(r => r.MinPrice)
                .GreaterThan(0)
                .WithMessage("Minimum price must be greater than 0.");
            RuleFor(r => r.MaxPrice)
                .GreaterThan(0)
                .WithMessage("Maximum price must be greater than 0.");
        }
    }
}
