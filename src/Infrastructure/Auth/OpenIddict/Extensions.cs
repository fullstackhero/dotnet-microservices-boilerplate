using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Validation.AspNetCore;

namespace FSH.Microservices.Infrastructure.Auth.OpenIddict;

public static class Extensions
{
    public static IServiceCollection RegisterOIDAuthValidation(this IServiceCollection services, IConfiguration config)
    {
        services.AddOptions<OpenIddictOptions>()
            .BindConfiguration(OpenIddictOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var authOptions = GetOIDAuthOptions(config);

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

    public static OpenIddictOptions GetOIDAuthOptions(this IConfiguration configuration)
        => configuration.GetSection(OpenIddictOptions.SectionName).Get<OpenIddictOptions>()!;
}
