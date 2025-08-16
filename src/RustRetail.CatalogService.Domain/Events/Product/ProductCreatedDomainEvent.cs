using Microsoft.AspNetCore.Http;
using RustRetail.SharedKernel.Domain.Events.Domain;

namespace RustRetail.CatalogService.Domain.Events.Product
{
    public class ProductCreatedDomainEvent : DomainEvent
    {
        public Guid ProductId { get; init; }
        public IFormFileCollection? ProductImages { get; init; }

        public ProductCreatedDomainEvent(Guid productId, IFormFileCollection? productImages)
        {
            ProductId = productId;
            ProductImages = productImages;
        }
    }
}
