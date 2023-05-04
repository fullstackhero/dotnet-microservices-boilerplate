using FSH.Framework.Core.Caching;
using FSH.Framework.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Framework.Infrastructure.Caching;
public static class Extensions
{
    public static IServiceCollection AddCachingService(this IServiceCollection services, IConfiguration config)
    {
        services.BindValidate<CachingOptions>(config);
        services.AddTransient<ICacheService, InMemoryCacheService>();
        services.AddMemoryCache();

        return services;
    }
}
