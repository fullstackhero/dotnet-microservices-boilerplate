using FluentPos.Catalog.Application;
using FluentPos.Catalog.Application.Products;
using FluentPos.Catalog.Infrastructure.Repositories;
using FSH.Framework.Infrastructure;
using FSH.Framework.Infrastructure.Auth.OpenId;
using FSH.Framework.Persistence.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FluentPos.Catalog.Infrastructure;
public static class Extensions
{
    public static void AddCatalogInfrastructure(this WebApplicationBuilder builder)
    {
        var applicationAssembly = typeof(CatalogApplication).Assembly;
        var policyNames = new List<string> { "catalog:read", "catalog:write" };
        builder.Services.AddOpenIdAuth(builder.Configuration, policyNames);
        builder.AddInfrastructure(applicationAssembly);
        builder.Services.AddMongoDbContext<MongoDbContext>(builder.Configuration);
        builder.Services.AddTransient<IProductRepository, ProductRepository>();
    }
    public static void UseCatalogInfrastructure(this WebApplication app)
    {
        app.UseInfrastructure(app.Environment);
    }
}
