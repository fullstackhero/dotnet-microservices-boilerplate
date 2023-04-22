using FluentPos.Catalog.Core.Products.Dtos;
using FSH.Microservices.Core.Database;
using FSH.Microservices.Core.Pagination;
using FSH.Microservices.Core.Services;
using FSH.Microservices.Persistence.NoSQL.Mongo;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace FluentPos.Catalog.Core.Products;

public interface IProductRepository : IRepository<Product, Guid>
{
    Task<PagedList<ProductDto>> GetPagedProductsAsync<ProductDto>(ProductsParametersDto parameters, CancellationToken cancellationToken = default);
}
public class ProductRepository : MongoRepository<Product, Guid>, IProductRepository
{
    private readonly IMongoDbContext _dbContext;
    private readonly IDateTimeService _dateTimeService;
    public ProductRepository(IMongoDbContext context, IDateTimeService dateTimeService) : base(context, dateTimeService)
    {
        _dbContext = context;
        _dateTimeService = dateTimeService;
    }

    public async Task<PagedList<ProductDto>> GetPagedProductsAsync<ProductDto>(ProductsParametersDto parameters, CancellationToken cancellationToken = default)
    {
        var queryable = _dbContext.GetCollection<Product>().AsQueryable();
        if (!string.IsNullOrEmpty(parameters.Keyword))
        {
            string keyword = parameters.Keyword.ToLower();
            queryable = queryable.Where(t => t.Name.ToLower().Contains(keyword)
            || t.Details.ToLower().Contains(keyword)
            || t.Code.ToLower().Contains(keyword));
        }
        queryable = queryable.OrderBy(p => p.CreatedOn);
        return await queryable.ApplyPagingAsync<Product, ProductDto>(parameters.PageNumber, parameters.PageSize, cancellationToken);
    }
}
