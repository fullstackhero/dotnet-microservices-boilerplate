using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Persistence.MongoDb;

public static class Extensions
{
    public static IServiceCollection AddMongoDbContext<TContext>(
        this IServiceCollection services, IConfiguration configuration)
        where TContext : MongoDbContext
    {
        return services.AddMongoDbContext<TContext, TContext>(configuration);
    }

    public static IServiceCollection AddMongoDbContext<TContextService, TContextImplementation>(
        this IServiceCollection services, IConfiguration configuration)
        where TContextService : IMongoDbContext
        where TContextImplementation : MongoDbContext, TContextService
    {
        services.Configure<MongoOptions>(configuration.GetSection(nameof(MongoOptions)));

        services.AddScoped(typeof(TContextService), typeof(TContextImplementation));
        services.AddScoped(typeof(TContextImplementation));

        services.AddScoped<IMongoDbContext>(sp => sp.GetRequiredService<TContextService>());

        services.AddTransient(typeof(IMongoRepository<,>), typeof(MongoRepository<,>));

        return services;
    }
}
