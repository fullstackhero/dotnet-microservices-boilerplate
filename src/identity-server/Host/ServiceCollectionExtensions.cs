using IDS.Host.Data;
using IDS.Host.Models;
using IDS.Host.Validators;
using Microsoft.AspNetCore.Identity;

namespace IDS.Host;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityServer(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(config =>
        {
            config.Password.RequiredLength = 6;
            config.Password.RequireDigit = false;
            config.Password.RequireNonAlphanumeric = false;
            config.Password.RequireUppercase = false;
        })
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

        var identityServerBuilder = services.AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;
        })
            .AddInMemoryIdentityResources(InMemoryConfiguration.IdentityResources)
            .AddInMemoryApiResources(InMemoryConfiguration.ApiResources)
            .AddInMemoryApiScopes(InMemoryConfiguration.ApiScopes)
            .AddInMemoryClients(InMemoryConfiguration.Clients)
            .AddAspNetIdentity<ApplicationUser>()
            .AddResourceOwnerValidator<UserValidator>();

        if (env.IsDevelopment())
        {
            identityServerBuilder.AddDeveloperSigningCredential();
        }

        return services;
    }
}
