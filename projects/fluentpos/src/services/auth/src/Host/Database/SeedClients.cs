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

        if (await manager.FindByClientIdAsync(Constants.Client) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = Constants.Client,
                Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.ClientCredentials,
                    Permissions.ResponseTypes.Token,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + Constants.CatalogScope
                }
            });
        }

        if (await manager.FindByClientIdAsync(Constants.CatalogResourceServer) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = Constants.CatalogResourceServer,
                ClientSecret = Constants.CatalogResourceServerSecret,
                Permissions =
                {
                    Permissions.Endpoints.Introspection
                }
            });
        }

        var scopesManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
        if (await scopesManager.FindByNameAsync(Constants.CatalogScope) is null)
        {
            await scopesManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = Constants.CatalogScope,
                Resources =
                {
                    Constants.CatalogResourceServer
                }
            });
        }

    }
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

