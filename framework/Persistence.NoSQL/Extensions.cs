using FSH.Framework.Core.Database;
using FSH.Framework.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Framework.Persistence.Mongo;
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
        var options = services.BindValidateReturn<MongoOptions>(configuration);
        if (string.IsNullOrEmpty(options.DatabaseName)) throw new ArgumentNullException(nameof(options.DatabaseName));
        if (string.IsNullOrEmpty(options.ConnectionString)) throw new ArgumentNullException(nameof(options.ConnectionString));
        services.AddScoped(typeof(TContextService), typeof(TContextImplementation));
        services.AddScoped(typeof(TContextImplementation));
        services.AddScoped<IMongoDbContext>(sp => sp.GetRequiredService<TContextService>());
        services.AddTransient(typeof(IRepository<,>), typeof(MongoRepository<,>));

        return services;
    }
}
