using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace FluentPos.Identity.Infrastructure.Persistence;

public class SeedClientsAndScopes : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public SeedClientsAndScopes(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        _ = await context.Database.EnsureCreatedAsync(cancellationToken);

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        if (await manager.FindByClientIdAsync(Constants.Client, cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = Constants.Client,
                ClientSecret = Constants.ClientSecret,
                DisplayName = Constants.ClientDisplayName,
                Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.ClientCredentials,
                    Permissions.ResponseTypes.Token,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + Constants.CatalogReadScope,
                    Permissions.Prefixes.Scope + Constants.CatalogWriteScope,
                    Permissions.Prefixes.Scope + Constants.CartReadScope,
                    Permissions.Prefixes.Scope + Constants.CartWriteScope
                }
            }, cancellationToken);
        }

        if (await manager.FindByClientIdAsync(Constants.GatewayResourceServer, cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = Constants.GatewayResourceServer,
                ClientSecret = Constants.GatewayResourceServerSecret,
                Permissions =
                {
                    Permissions.Endpoints.Introspection
                }
            }, cancellationToken);
        }

        if (await manager.FindByClientIdAsync(Constants.CatalogResourceServer, cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = Constants.CatalogResourceServer,
                ClientSecret = Constants.CatalogResourceServerSecret,
                Permissions =
                {
                    Permissions.Endpoints.Introspection
                }
            }, cancellationToken);
        }

        var scopesManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

        if (await scopesManager.FindByNameAsync(Constants.CatalogWriteScope, cancellationToken) is null)
        {
            await scopesManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = Constants.CatalogWriteScope,
                Resources =
                {
                    Constants.CatalogResourceServer,
                    Constants.GatewayResourceServer
                }
            }, cancellationToken);
        }

        if (await scopesManager.FindByNameAsync(Constants.CatalogReadScope, cancellationToken) is null)
        {
            await scopesManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = Constants.CatalogReadScope,
                Resources =
                {
                    Constants.CatalogResourceServer,
                    Constants.GatewayResourceServer
                }
            }, cancellationToken);
        }

        if (await scopesManager.FindByNameAsync(Constants.CartWriteScope, cancellationToken) is null)
        {
            await scopesManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = Constants.CartWriteScope,
                Resources =
                {
                    Constants.CartResourceServer,
                    Constants.GatewayResourceServer
                }
            }, cancellationToken);
        }

        if (await scopesManager.FindByNameAsync(Constants.CartReadScope, cancellationToken) is null)
        {
            await scopesManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = Constants.CartReadScope,
                Resources =
                {
                    Constants.CartResourceServer,
                    Constants.GatewayResourceServer
                }
            }, cancellationToken);
        }
    }
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
