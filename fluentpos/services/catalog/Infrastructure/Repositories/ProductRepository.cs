using FluentPos.Catalog.Application.Products;
using FluentPos.Catalog.Application.Products.Dtos;
using FluentPos.Catalog.Domain.Products;
using FSH.Framework.Core.Pagination;
using FSH.Framework.Core.Services;
using FSH.Framework.Persistence.Mongo;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace FluentPos.Catalog.Infrastructure.Repositories;

public class ProductRepository : MongoRepository<Product, Guid>, IProductRepository
{
    private readonly IMongoDbContext _dbContext;
    public ProductRepository(IMongoDbContext context, IDateTimeService dateTimeService) : base(context, dateTimeService)
    {
        _dbContext = context;
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
