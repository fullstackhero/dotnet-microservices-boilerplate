using FSH.Framework.Core.Exceptions;
using FSH.Framework.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Framework.Infrastructure.Auth.OpenId;
public static class Extensions
{
    public static IServiceCollection AddOpenIdAuth(this IServiceCollection services, IConfiguration config, List<string> policyNames)
    {
        var authOptions = services.BindValidateReturn<OpenIdOptions>(config);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Authority = authOptions.Authority;
            options.Audience = authOptions.Audience;
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                RequireAudience = true,
                ValidateAudience = true,
            };
            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    if (!context.Response.HasStarted)
                    {
                        throw new UnauthorizedException(context.Error!, context.ErrorDescription!);
                    }

                    return Task.CompletedTask;
                },
                OnForbidden = _ => throw new ForbiddenException()
            };
        });

        if (policyNames?.Count > 0)
        {
            services.AddAuthorization(options =>
            {
                foreach (string policyName in policyNames)
                {
                    options.AddPolicy(policyName, policy => policy.Requirements.Add(new HasScopeRequirement(policyName, authOptions.Authority!)));
                }
            });
        }

        services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
        return services;
    }
}
