using FSH.Microservices.Infrastructure.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Validation.AspNetCore;
using System.Reflection;

namespace FSH.Microservices.Infrastructure.Auth.OpenIddict;

public static class Extensions
{
    public static IServiceCollection AddOpenIddictValidation(this IServiceCollection services, IConfiguration config)
    {
        var authOptions = services.ValidateAndLoad<OpenIddictOptions>(config);

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

    public static void ConfigureOpenIddictServer<T>(this WebApplicationBuilder builder, string connectionName = "DefaultConnection") where T : DbContext
    {
        builder.Services.AddOpenIddict()
        .AddCore(options =>
        {
            options.UseEntityFrameworkCore().UseDbContext<T>();
        })
        .AddServer(options =>
        {
            options.SetAuthorizationEndpointUris("/connect/authorize")
                   .SetIntrospectionEndpointUris("/connect/introspect")
                   .SetTokenEndpointUris("/connect/token");
            options.AllowClientCredentialsFlow();
            if (builder.Environment.IsDevelopment())
            {
                // Disable Payload Encryption in JWTs
                options.DisableAccessTokenEncryption();
                options.AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate();
            }
            options.UseAspNetCore().EnableTokenEndpointPassthrough();
        });

        string? connectionString = builder.Configuration.GetConnectionString(connectionName);
        Assembly assembly = Assembly.GetExecutingAssembly();

        builder.Services.AddDbContext<T>(options =>
        {
            options.UseNpgsql(connectionString, m => m.MigrationsAssembly(assembly.FullName));
            options.UseOpenIddict();
        });
    }
}
