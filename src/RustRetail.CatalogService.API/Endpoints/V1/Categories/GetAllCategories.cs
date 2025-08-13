using MediatR;
using RustRetail.CatalogService.API.Common;
using RustRetail.CatalogService.Application.Categories.GetAllCategories;
using RustRetail.CatalogService.Contracts.Categories;
using RustRetail.SharedInfrastructure.MinimalApi;

namespace RustRetail.CatalogService.API.Endpoints.V1.Categories
{
    public class GetAllCategories : IEndpoint
    {
        const string Route = $"{Resources.Categories}";

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet(Route, Handle)
                .WithTags(Tags.Categories)
                .AllowAnonymous()
                .MapToApiVersion(1);
        }

        static async Task<IResult> Handle(
            HttpContext httpContext,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var query = new GetAllCategoriesQuery();
            var result = await sender.Send(query, cancellationToken);
            return result.IsSuccess
                ? Results.Ok(new SuccessResultWrapper<List<CategorySummaryDto>>(result, httpContext))
                : ResultExtension.HandleFailure(result, httpContext);
        }
    }
}
