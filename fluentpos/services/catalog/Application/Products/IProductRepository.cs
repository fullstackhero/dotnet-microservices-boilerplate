using FluentPos.Catalog.Application.Products.Dtos;
using FluentPos.Catalog.Domain.Products;
using FSH.Framework.Core.Database;
using FSH.Framework.Core.Pagination;

namespace FluentPos.Catalog.Application.Products;
public interface IProductRepository : IRepository<Product, Guid>
{
    Task<PagedList<ProductDto>> GetPagedProductsAsync<ProductDto>(ProductsParametersDto parameters, CancellationToken cancellationToken = default);
}