using Microsoft.AspNetCore.Http;
using RustRetail.CatalogService.Domain.Abstractions.Database;
using RustRetail.CatalogService.Domain.Entities;
using RustRetail.SharedApplication.Abstractions;
using RustRetail.SharedKernel.Domain.Abstractions;
using System.Text;

namespace RustRetail.CatalogService.Application.Products.CreateProduct
{
    internal class CreateProductCommandHandler(
        ICatalogDbContext dbContext)
        : ICommandHandler<CreateProductCommand>
    {
        public async Task<Result> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = Product.Create(request.Name,
                request.Description,
                request.Price,
                request.SKU,
                request.CategoryId,
                request.BrandId);
            // Handle images generation
            product.Images = ProcessProductImages(request.Images!, product.Id.ToString(), product.Name);

            await dbContext.Products.InsertOneAsync(product, cancellationToken: cancellationToken);

            // To do: Add domain event to handle side effects (uploading images,...)

            return Result.Success();
        }

        List<ProductImage> ProcessProductImages(IFormFileCollection images, string productId, string productName = "")
        {
            var productImages = new List<ProductImage>();
            StringBuilder imagePath = new StringBuilder();
            for (int i = 0; i < images.Count; i++)
            {
                int imagePosition = i + 1;
                imagePath.Clear();
                imagePath.Append($"{productId}/{imagePosition}{Path.GetExtension(images[i].FileName)}");
                productImages.Add(ProductImage.Create(imagePath.ToString(),
                    GenerateImageAltText(productName, imagePosition.ToString())));
            }
            return productImages;
        }

        string GenerateImageAltText(string productName, string imageName)
        {
            return $"Image {imageName} of product {productName}.";
        }
    }
}
