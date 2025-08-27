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

            CreateIndexes();
        }

        public IMongoCollection<Category> Categories => _database.GetCollection<Category>(CatalogDbConstants.CategoryCollectionName);
        public IMongoCollection<Brand> Brands => _database.GetCollection<Brand>(CatalogDbConstants.BrandCollectionName);
        public IMongoCollection<Product> Products => _database.GetCollection<Product>(CatalogDbConstants.ProductCollectionName);

        private void CreateIndexes()
        {
            CreateBrandIndexes();
            CreateCategoryIndexes();
            CreateProductIndexes();
        }

        void CreateProductIndexes()
        {
            var indexKeys = Builders<Product>.IndexKeys;
            var indexes = new List<CreateIndexModel<Product>>
            {
                new CreateIndexModel<Product>(
                    indexKeys.Text(p => p.Name).Text(p => p.Description)),
                new CreateIndexModel<Product>(
                    indexKeys.Ascending(p => p.SKU), new CreateIndexOptions { Unique = true }),
                new CreateIndexModel<Product>(
                    indexKeys.Ascending(p => p.CategoryId).Ascending(p => p.BrandId)),
                new CreateIndexModel<Product>(
                    indexKeys.Ascending(p => p.CategoryId).Ascending(p => p.Price))
            };
            Products.Indexes.CreateMany(indexes);
            _logger.LogInformation("Indexes created for Products collection.");
        }

        void CreateBrandIndexes()
        {
            var indexKeys = Builders<Brand>.IndexKeys;
            var indexes = new List<CreateIndexModel<Brand>>
            {
                new CreateIndexModel<Brand>(
                    indexKeys.Text(b => b.Name).Text(b => b.Description))
            };
            Brands.Indexes.CreateMany(indexes);
            _logger.LogInformation("Indexes created for Brands collection.");
        }

        void CreateCategoryIndexes()
        {
            var indexKeys = Builders<Category>.IndexKeys;
            var indexes = new List<CreateIndexModel<Category>>
            {
                new CreateIndexModel<Category>(
                    indexKeys.Text(c => c.Name).Text(c => c.Description)),
                new CreateIndexModel<Category>(
                    indexKeys.Ascending(c => c.ParentCategoryId))
            };
            Categories.Indexes.CreateMany(indexes);
            _logger.LogInformation("Indexes created for Categories collection.");
        }
    }
}
