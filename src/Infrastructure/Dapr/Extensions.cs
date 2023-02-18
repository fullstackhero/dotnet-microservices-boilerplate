using System.Text.Json;
using FSH.Infrastructure.Bus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Infrastructure.Dapr;

public static class Extensions
{
    public static IServiceCollection RegisterDapr(this IServiceCollection services, IConfiguration config)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
        };
        services.AddDaprClient(client => client.UseJsonSerializationOptions(options));
        services.RegisterMessageBroker(config, Brokers.DaprManaged);
        return services;
    }
}
