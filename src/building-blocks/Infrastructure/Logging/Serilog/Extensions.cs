using Microsoft.Extensions.DependencyInjection;

namespace FSH.Infrastructure.Logging.Serilog;

public static class Extensions
{
    internal static IServiceCollection RegisterSerilog(IServiceCollection services)
    {
        return services;
    }
}
