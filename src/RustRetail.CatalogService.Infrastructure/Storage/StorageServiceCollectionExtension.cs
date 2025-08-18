using Amazon;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RustRetail.CatalogService.Application.Abstractions.Services;
using RustRetail.CatalogService.Infrastructure.Storage.S3;

namespace RustRetail.CatalogService.Infrastructure.Storage
{
    internal static class StorageServiceCollectionExtension
    {
        internal static IServiceCollection AddStorageServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.AddAwsS3StorageService(configuration);
        }

        private static IServiceCollection AddAwsS3StorageService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IAmazonS3>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<AwsS3Options>>().Value;
                var config = new AmazonS3Config
                {
                    RegionEndpoint = RegionEndpoint.GetBySystemName(options.Region)
                };
                return new AmazonS3Client(options.AccessKey, options.SecretKey, config);
            });
            services.AddSingleton<IFileStorage, AwsS3FileStorage>();

            return services;
        }
    }
}
