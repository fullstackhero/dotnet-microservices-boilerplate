using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace FluentPOS.Auth.Host.Database;

public class SeedClients : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public SeedClients(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        await context.Database.EnsureCreatedAsync();

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        if (await manager.FindByClientIdAsync("catalog.api") is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "catalog.api",
                ClientSecret = "846B62D0-DEF9-4215-A99D-86E6B8DAB342",
                Permissions =
                {
                    Permissions.Endpoints.Introspection,
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.ClientCredentials,
                    Permissions.ResponseTypes.Token,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + "catalog.api"
                }
            });
        }

        var scopesManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

        if (await scopesManager.FindByNameAsync("catalog.api") is null)
        {
            await scopesManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = "catalog.api",
                Resources =
                {
                    "catalog.api"
                }
            });
        }

    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

