using MediatR;
using Microsoft.AspNetCore.Mvc;
using RustRetail.CatalogService.API.Common;
using RustRetail.CatalogService.Application.Products.GetById;
using RustRetail.CatalogService.Contracts.Products.GetById;
using RustRetail.SharedInfrastructure.MinimalApi;

namespace RustRetail.CatalogService.API.Endpoints.V1.Products
{
    public class GetById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("products/{id:guid}", Handle)
                .WithTags(Tags.Products)
                .AllowAnonymous()
                .MapToApiVersion(1);
        }

        static async Task<IResult> Handle(
            [FromRoute] Guid id,
            HttpContext httpContext,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var query = new GetProductByIdQuery(id);
            var result = await sender.Send(query, cancellationToken);
            return result.IsSuccess
                ? Results.Ok(new SuccessResultWrapper<GetProductByIdResponse>(result, httpContext))
                : ResultExtension.HandleFailure(result, httpContext);
        }
    }
}
