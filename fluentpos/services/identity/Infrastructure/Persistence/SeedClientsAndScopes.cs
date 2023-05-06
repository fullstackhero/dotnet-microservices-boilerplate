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
        await context.Database.EnsureCreatedAsync();

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        if (await manager.FindByClientIdAsync(Constants.Client) is null)
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
                    Permissions.Prefixes.Scope + Constants.GatewayScope,
                    Permissions.Prefixes.Scope + Constants.CatalogScope
                }
            }); ;
        }

        if (await manager.FindByClientIdAsync(Constants.GatewayResourceServer) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = Constants.GatewayResourceServer,
                ClientSecret = Constants.GatewayResourceServerSecret,
                Permissions =
                {
                    Permissions.Endpoints.Introspection
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
        if (await scopesManager.FindByNameAsync(Constants.GatewayScope) is null)
        {
            await scopesManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = Constants.GatewayScope,
                Resources =
                {
                    Constants.CatalogResourceServer,
                    Constants.GatewayResourceServer
                }
            });
        }
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

