using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using RustRetail.CatalogService.Application.Abstractions.Services;
using RustRetail.CatalogService.Application.Common.Utilities;
using RustRetail.CatalogService.Domain.Abstractions.Database;
using RustRetail.CatalogService.Domain.Entities;
using RustRetail.CatalogService.Domain.Events.Product;
using RustRetail.SharedApplication.Abstractions;

namespace RustRetail.CatalogService.Application.Products.CreateProduct
{
    internal class ProductCreatedDomainEventHandler(
        ICatalogDbContext dbContext,
        IFileStorage fileStorage)
        : IDomainEventHandler<DomainEventNotification<ProductCreatedDomainEvent>>
    {
        public async Task Handle(
            DomainEventNotification<ProductCreatedDomainEvent> notification,
            CancellationToken cancellationToken)
        {
            if (notification.DomainEvent.ProductImages == null || notification.DomainEvent.ProductImages.Count == 0) return;
            var imageKeys = GenerateImageKeys(notification.DomainEvent.ProductImages, notification.DomainEvent.ProductId.ToString());
            var uploadResults = await fileStorage.UploadManyAsync(notification.DomainEvent.ProductImages, imageKeys, cancellationToken);

            var filter = Builders<Product>.Filter.Eq(p => p.Id, notification.DomainEvent.ProductId);
            var productImages = GenerateProductImages(uploadResults, notification.DomainEvent.ProductName);
            var update = Builders<Product>.Update.Set(p => p.Images, productImages);
            await dbContext.Products.UpdateOneAsync(filter, update);
        }

        List<string> GenerateImageKeys(IFormFileCollection images, string productId)
        {
            var results = new List<string>();
            for (int i = 0; i < images.Count; i++)
            {
                results.Add(S3ImageKeyBuilder.BuildProductImageKey(productId, $"{i + 1}{Path.GetExtension(images[i].FileName)}"));
            }
            return results;
        }

        List<ProductImage> GenerateProductImages(IReadOnlyList<StorageObject> storageObjects, string productName)
        {
            var results = new List<ProductImage>();
            for (int i = 0; i < storageObjects.Count; i++)
            {
                results.Add(ProductImage.Create(
                    storageObjects[i].Url,
                    ProductImage.GenerateImageAltText(productName, $"{i + 1}")));
            }
            return results;
        }
    }
}
