using FSH.Core.Common;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Infrastructure.Caching;

public static class Extensions
{
    public static IServiceCollection AddCaching(this IServiceCollection services)
    {
        //services.AddDistributedMemoryCache();
        services.AddTransient<ICacheService, DaprCacheService>();
        return services;
    }
}
