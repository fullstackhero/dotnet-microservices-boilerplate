using FluentPos.Cart.Application;
using FluentPos.Cart.Infrastructure.Repositories;
using FSH.Framework.Core.Events;
using FSH.Framework.Infrastructure;
using FSH.Framework.Infrastructure.Auth.OpenId;
using FSH.Framework.Infrastructure.Messaging;
using MassTransit;
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

        builder.Services.AddMassTransit(config =>
        {
            config.AddConsumers(applicationAssembly);
            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(builder.Configuration["RabbitMqOptions:Host"]);
                cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter("cart", false));
            });
        });

        builder.AddInfrastructure(applicationAssembly);
        builder.Services.AddTransient<IEventPublisher, EventPublisher>();
        builder.Services.AddTransient<ICartRepository, CartRepository>();
    }
    public static void UseCartInfrastructure(this WebApplication app)
    {
        app.UseInfrastructure(app.Environment);
    }
}
