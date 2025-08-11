using Microsoft.AspNetCore.Http.Features;
using RustRetail.SharedKernel.Domain.Abstractions;
using System.Diagnostics;

namespace RustRetail.CatalogService.API.Common
{
    public class SuccessResultWrapper
    {
        public string RequestId { get; init; } = string.Empty;
        public string TraceId { get; init; } = string.Empty;
        public bool IsSuccess { get; init; } = true;

        public SuccessResultWrapper(Result result, HttpContext httpContext)
        {
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException("Cannot construct result with metadata from failure result");
            }
            IsSuccess = result.IsSuccess;
            RequestId = httpContext.TraceIdentifier;
            Activity? activity = httpContext.Features.Get<IHttpActivityFeature>()?.Activity;
            TraceId = activity != null ? activity.Id! : string.Empty;
        }
    }

    public class SuccessResultWrapper<T> : SuccessResultWrapper
    {
        public T Data { get; init; }

        public SuccessResultWrapper(Result<T> result, HttpContext httpContext)
            : base(result, httpContext)
        {
            Data = result.Value;
        }

        public SuccessResultWrapper(Result result, HttpContext httpContext, T data)
            : base(result, httpContext)
        {
            Data = data;
        }
    }
}
