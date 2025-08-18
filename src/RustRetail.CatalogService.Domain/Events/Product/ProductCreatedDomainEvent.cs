using Microsoft.AspNetCore.Http;
using RustRetail.SharedKernel.Domain.Events.Domain;

namespace RustRetail.CatalogService.Domain.Events.Product
{
    public class ProductCreatedDomainEvent : DomainEvent
    {
        public Guid ProductId { get; init; }
        public string ProductName { get; init; }
        public IFormFileCollection? ProductImages { get; init; }

        public ProductCreatedDomainEvent(Guid productId,
            string productName,
            IFormFileCollection? productImages)
        {
            ProductId = productId;
            ProductName = productName;
            ProductImages = productImages;
        }
    }
}
