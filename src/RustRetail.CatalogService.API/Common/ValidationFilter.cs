using FluentValidation;
using RustRetail.CatalogService.Domain.Errors.Common;
using RustRetail.SharedKernel.Domain.Abstractions;

namespace RustRetail.CatalogService.API.Common
{
    public class ValidationFilter<TRequest> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            var validators = context.HttpContext.RequestServices.GetServices<IValidator<TRequest>>();
            if (validators is null || validators.Count() <= 0)
                return await next(context);

            var request = context.Arguments.OfType<TRequest>().FirstOrDefault();
            if (request is null)
                return ResultExtension.HandleFailure(
                    Result.Failure(ValidationErrors.RequestBodyMissing),
                    context.HttpContext);

            var validationResults = await Task.WhenAll(
                validators.Select(validator => validator.ValidateAsync(request)));
            var validationErrors = validationResults
                .SelectMany(vr => vr.Errors)
                .Where(validationFailure => validationFailure is not null)
                .Select(failure => new { Field = failure.PropertyName, Description = failure.ErrorMessage })
                .Distinct()
                .ToArray();
            var error = Error.Validation(ValidationErrors.InvalidRequest.Code,
                ValidationErrors.InvalidRequest.Description,
                validationErrors);
            if (validationResults.Any(vr => !vr.IsValid) && validationErrors.Any())
            {
                return ResultExtension.HandleFailure(
                    Result.Failure(error),
                    context.HttpContext);
            }

            return await next(context);
        }
    }
}
