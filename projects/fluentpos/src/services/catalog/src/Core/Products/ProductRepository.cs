using FSH.Microservices.Core.Database;
using FSH.Microservices.Persistence.NoSQL.Mongo;

namespace FluentPos.Catalog.Core.Products;

public interface IProductRepository : IRepository<Product, Guid>
{
}
public class ProductRepository : MongoRepository<Product, Guid>, IProductRepository
{
    public ProductRepository(IMongoDbContext context) : base(context)
    {
    }
}
