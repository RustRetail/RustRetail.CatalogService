using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RustRetail.CatalogService.API.Common;
using RustRetail.CatalogService.Application.Common.Utilities;
using RustRetail.CatalogService.Application.Products.CreateProduct;
using RustRetail.CatalogService.Contracts.Products.CreateProduct;
using RustRetail.SharedInfrastructure.MinimalApi;

namespace RustRetail.CatalogService.API.Endpoints.V1.Products
{
    public class CreateProduct : IEndpoint
    {
        const string Route = $"{Resources.Products}";

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost(Route, Handle)
                .WithTags(Tags.Products)
                .AllowAnonymous()
                .MapToApiVersion(1)
                .DisableAntiforgery()
                .AddEndpointFilter<ValidationFilter<CreateProductRequest>>();
        }

        static async Task<IResult> Handle(
            [FromForm] CreateProductRequest? request,
            HttpContext httpContext,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new CreateProductCommand(request!.Product.Name,
                request.Product.Description,
                request.Product.Price,
                request.Product.SKU,
                request.Product.CategoryId,
                request.Product.BrandId,
                request.Images);
            var result = await sender.Send(command, cancellationToken);
            return result.IsSuccess
                ? Results.Ok(new SuccessResultWrapper(result, httpContext))
                : ResultExtension.HandleFailure(result, httpContext);
        }
    }

    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(p => p.Product.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Product's name is required.")
                .MaximumLength(200)
                .WithMessage("Product's name cannot exceed 200 characters.");

            RuleFor(p => p.Product.Description)
                .NotNull()
                .NotEmpty()
                .WithMessage("Product's description is required.")
                .MaximumLength(1000)
                .WithMessage("Product's description cannot exceed 1000 characters.");

            RuleFor(p => p.Product.Price)
                .NotEmpty()
                .NotNull()
                .WithMessage("Product's price is required.")
                .Must(p => p > 0)
                .WithMessage("Product's price must be larger than 0.");

            RuleFor(p => p.Product.SKU)
                .NotEmpty()
                .NotNull()
                .WithMessage("Product's SKU is required.")
                .MaximumLength(20)
                .WithMessage("Product's SKU cannot exceed 20 characters.");

            RuleFor(p => p.Images)
                .NotNull()
                .NotEmpty()
                .WithMessage("Product images list must be valid and contains image(s).")
                .Must(i => i!.Count() > 0).WithMessage("Product must contains at least 1 image.")
                .Must(i => i!.Count() <= 10).WithMessage("Product cannot have more than 10 images.");

            RuleForEach(p => p.Images)
                .NotNull().WithMessage("Product's image cannot be null.")
                .Must(ImageChecker.IsImageExtensionAllowed).WithMessage("Only support image with '.png', '.jpg' and '.jpeg' extensions.")
                .Must(ImageChecker.IsImageSizeAllowed).WithMessage("Product image size cannot exceed 5 MB.")
                .Must(ImageChecker.IsImageMimeTypeAllowed).WithMessage("Only support image with 'image/png' and 'image/jpeg' content types.");
        }
    }
}
