using FSH.Core.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FSH.Persistence.EfCore;

public static class Extensions
{
    public static IServiceCollection RegisterContext<T>(this IServiceCollection services, IConfiguration configuration, Database databaseChoice, string connectionStringName)
        where T : DbContext
    {
        var assemblyName = typeof(T).Assembly.GetName().Name;
        var connectionString = configuration.GetConnectionString(connectionStringName);
        switch (databaseChoice)
        {
            case Database.PostgreSQL:
                services.AddDbContext<T>(o => o.UseNpgsql(connectionString, m => m.MigrationsAssembly(assemblyName)));
                break;
            case Database.SQLServer:
                break;
            default:
                break;
        }
        return services;
    }

    public static IApplicationBuilder ConfigureMigrations<T>(this IApplicationBuilder applicationBuilder, IWebHostEnvironment webHostEnvironment)
    where T : DbContext
    {
        MigrateDatabaseAsync<T>(applicationBuilder.ApplicationServices).GetAwaiter().GetResult();
        if (webHostEnvironment.IsDevelopment() || webHostEnvironment.EnvironmentName == "docker")
        {
            SeedDataAsync(applicationBuilder.ApplicationServices).GetAwaiter().GetResult();
        }
        return applicationBuilder;
    }
    private static async Task MigrateDatabaseAsync<T>(IServiceProvider serviceProvider)
        where T : DbContext
    {
        using var scope = serviceProvider.CreateScope();
        var logService = scope.ServiceProvider.GetRequiredService<ILogger<Database>>();
        var context = scope.ServiceProvider.GetRequiredService<T>();
        if (context.Database.GetPendingMigrations().Any())
        {
            logService.LogInformation("Applying Migrations for {typeName}.", typeof(T).Name);
            await context.Database.MigrateAsync();
        }
    }
    private static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        foreach (var seeder in scope.ServiceProvider.GetServices<IDataSeeder>())
        {
            await seeder.SeedAllAsync();
        }
    }
}
