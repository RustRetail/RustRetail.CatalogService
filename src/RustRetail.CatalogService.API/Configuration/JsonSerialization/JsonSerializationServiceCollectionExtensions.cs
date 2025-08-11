using System.Text.Json.Serialization;

namespace RustRetail.CatalogService.API.Configuration.JsonSerialization
{
    internal static class JsonSerializationServiceCollectionExtensions
    {
        internal static IServiceCollection AddJsonSerializationConfiguration(
            this IServiceCollection services)
        {
            return services.ConfigureHttpJsonOptions(options =>
            {
                // Serialize enum as string
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }
    }
}
