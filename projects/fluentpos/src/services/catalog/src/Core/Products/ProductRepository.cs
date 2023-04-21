using FSH.Microservices.Core.Database;
using FSH.Microservices.Core.Pagination;
using FSH.Microservices.Persistence.NoSQL.Mongo;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace FluentPos.Catalog.Core.Products;

public interface IProductRepository : IRepository<Product, Guid>
{
    Task<PagedList<ProductDto>> GetPagedProductsAsync<ProductDto>(int pageNumber, int pageSize, string searchKey, CancellationToken cancellationToken = default);
}
public class ProductRepository : MongoRepository<Product, Guid>, IProductRepository
{
    private readonly IMongoDbContext _dbContext;
    public ProductRepository(IMongoDbContext context) : base(context)
    {
        _dbContext = context;
    }

    public async Task<PagedList<ProductDto>> GetPagedProductsAsync<ProductDto>(int pageNumber, int pageSize, string searchKey, CancellationToken cancellationToken = default)
    {
        var queryable = _dbContext.GetCollection<Product>().AsQueryable();
        if (!string.IsNullOrEmpty(searchKey))
        {
            queryable = queryable.Where(t => t.Name.ToLower().Contains(searchKey.ToLower())
            || t.Details.ToLower().Contains(searchKey.ToLower())
            || t.Code.ToLower().Contains(searchKey.ToLower()));
        }
        queryable = queryable.OrderBy(p => p.CreatedOn);
        return await queryable.ApplyPagingAsync<Product, ProductDto>(pageNumber, pageSize, cancellationToken);
    }
}
