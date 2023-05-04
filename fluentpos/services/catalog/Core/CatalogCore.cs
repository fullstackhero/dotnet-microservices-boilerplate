using FluentPos.Catalog.Core.Products;
using Microsoft.Extensions.DependencyInjection;

namespace FluentPos.Catalog.Core;
public static class CatalogCore
{
    public static void AddCoreCatalogService(this IServiceCollection services)
    {
        services.AddTransient<IProductRepository, ProductRepository>();
    }
}
