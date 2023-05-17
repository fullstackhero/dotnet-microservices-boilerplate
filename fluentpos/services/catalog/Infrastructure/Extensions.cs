using FluentPos.Catalog.Application.Products;
using FluentPos.Catalog.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FluentPos.Catalog.Infrastructure;
public static class Extensions
{
    public static void AddCatalogRepositories(this IServiceCollection services)
    {
        services.AddTransient<IProductRepository, ProductRepository>();
    }
}
