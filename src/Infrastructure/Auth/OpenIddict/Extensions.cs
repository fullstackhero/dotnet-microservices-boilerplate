using FSH.Microservices.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Validation.AspNetCore;

namespace FSH.Microservices.Infrastructure.Auth.OpenIddict;

public static class Extensions
{
    public static IServiceCollection RegisterOIDAuthValidation(this IServiceCollection services, IConfiguration config)
    {
        var authOptions = services.AddLoadValidateOptions<OpenIddictOptions>(config);

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
}
