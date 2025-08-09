namespace RustRetail.CatalogService.Persistence.Database
{
    public class MongoDbSettings
    {
        public const string SectionName = "MongoDbSettings";

        public string ConnectionString { get; set; } = null!;
        public string Database { get; set; } = null!;
    }
}
