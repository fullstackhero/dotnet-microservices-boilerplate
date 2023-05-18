using FluentPos.Cart.Application;
using FluentPos.Cart.Infrastructure.Repositories;
using FSH.Framework.Infrastructure;
using FSH.Framework.Infrastructure.Auth.OpenId;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FluentPos.Cart.Infrastructure;

public static class Extensions
{
    public static void AddCartInfrastructure(this WebApplicationBuilder builder)
    {
        var applicationAssembly = typeof(CartApplication).Assembly;
        var policyNames = new List<string> { "cart:read", "cart:write" };
        builder.Services.AddOpenIdAuth(builder.Configuration, policyNames);
        builder.AddInfrastructure(applicationAssembly);
        builder.Services.AddTransient<ICartRepository, CartRepository>();
    }
    public static void UseCartInfrastructure(this WebApplication app)
    {
        app.UseInfrastructure(app.Environment);
    }
}
