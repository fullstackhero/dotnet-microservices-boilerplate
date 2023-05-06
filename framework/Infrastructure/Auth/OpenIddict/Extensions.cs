using System.Reflection;
using FSH.Framework.Infrastructure.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Validation.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace FSH.Framework.Infrastructure.Auth.OpenIddict;

public static class Extensions
{
    public static IServiceCollection AddAuthValidation(this IServiceCollection services, IConfiguration config)
    {
        var authOptions = services.BindValidateReturn<OpenIddictOptions>(config);

        services.AddOpenIddict()
        .AddValidation(options =>
        {
            options.SetIssuer(authOptions.IssuerUrl!);
            options.UseIntrospection()
                   .SetClientId(authOptions.ClientId!)
                   .SetClientSecret(authOptions.ClientSecret!);
            options.UseSystemNetHttp();
            options.UseAspNetCore();
        });

        services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        services.AddAuthorization();
        return services;
    }

    public static void ConfigureAuthServer<T>(this WebApplicationBuilder builder, Assembly dbContextAssembly, string connectionName = "DefaultConnection") where T : DbContext
    {
        builder.Services.AddOpenIddict()
        .AddCore(options => options.UseEntityFrameworkCore().UseDbContext<T>())
        .AddServer(options =>
        {
            options.SetAuthorizationEndpointUris("/connect/authorize")
                   .SetIntrospectionEndpointUris("/connect/introspect")
                   .SetUserinfoEndpointUris("connect/userinfo")
                   .SetTokenEndpointUris("/connect/token");
            options.AllowClientCredentialsFlow();
            options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);
            options.DisableAccessTokenEncryption();
            options.AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate();
            options.UseAspNetCore().EnableTokenEndpointPassthrough().DisableTransportSecurityRequirement();
        })
        .AddValidation(options =>
        {
            options.UseLocalServer();
            options.UseAspNetCore();
        });

        builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        builder.Services.AddAuthorization();

        string? connectionString = builder.Configuration.GetConnectionString(connectionName);
        if (!builder.Environment.IsDevelopment() && connectionString == null)
            throw new ArgumentNullException(nameof(connectionString));

        builder.Services.AddDbContext<T>(options =>
        {
            if (builder.Environment.IsDevelopment())
            {
                options.UseInMemoryDatabase("authDb");
            }
            else
            {
                options.UseNpgsql(connectionString, m => m.MigrationsAssembly(dbContextAssembly.FullName));
            }
            options.UseOpenIddict();
        });
    }
}
