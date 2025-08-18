using RustRetail.CatalogService.Contracts.Products.GetById;
using RustRetail.SharedApplication.Abstractions;

namespace RustRetail.CatalogService.Application.Products.GetById
{
    public record GetProductByIdQuery(
        Guid Id) : IQuery<GetProductByIdResponse>;
}
