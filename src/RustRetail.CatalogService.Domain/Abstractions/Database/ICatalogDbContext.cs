using MongoDB.Driver;
using RustRetail.CatalogService.Domain.Entities;

namespace RustRetail.CatalogService.Domain.Abstractions.Database
{
    public interface ICatalogDbContext
    {
        IMongoCollection<Category> Categories { get; }
        IMongoCollection<Brand> Brands { get; }
        IMongoCollection<Product> Products { get; }
    }
}
