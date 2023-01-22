using FSH.Infrastructure.Bus.Dapr;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Infrastructure.Bus;

public static class Extensions
{
    internal static IServiceCollection RegisterMessageBroker(this IServiceCollection services,
    IConfiguration config, Brokers broker = Brokers.DaprManaged)
    {
        switch (broker)
        {
            case Brokers.DaprManaged:
                services.Configure<DaprEventBusOptions>(config.GetSection(DaprEventBusOptions.Name));
                services.AddScoped<IEventBus, DaprEventBus>();
                break;
        }

        return services;
    }
}
