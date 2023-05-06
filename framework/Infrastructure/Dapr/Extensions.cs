using FSH.Framework.Core.Events;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Framework.Infrastructure.Dapr;
public static class Extensions
{
    internal static IServiceCollection AddDaprBuildingBlocks(this IServiceCollection services)
    {
        services.AddDaprClient();
        services.AddScoped<IEventBus, DaprEventBus>();
        return services;
    }
}
