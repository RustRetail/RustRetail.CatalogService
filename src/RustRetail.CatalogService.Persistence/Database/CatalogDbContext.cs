using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RustRetail.CatalogService.Domain.Abstractions.Database;
using RustRetail.CatalogService.Domain.Constants.Database;
using RustRetail.CatalogService.Domain.Entities;

namespace RustRetail.CatalogService.Persistence.Database
{
    public class CatalogDbContext : ICatalogDbContext
    {
        readonly IMongoDatabase _database;
        readonly ILogger<CatalogDbContext> _logger;

        public CatalogDbContext(IMongoClient client,
            IOptions<MongoDbSettings> settings,
            ILogger<CatalogDbContext> logger)
        {
            _logger = logger;
            string dbName = settings.Value.Database;
            _database = client.GetDatabase(dbName);
            _logger.LogInformation("MongoDB database initialized: {Database}", dbName);
        }

        public IMongoCollection<Category> Categories => _database.GetCollection<Category>(CatalogDbConstants.CategoryCollectionName);
        public IMongoCollection<Brand> Brands => _database.GetCollection<Brand>(CatalogDbConstants.BrandCollectionName);
        public IMongoCollection<Product> Products => _database.GetCollection<Product>(CatalogDbConstants.ProductCollectionName);
    }
}
