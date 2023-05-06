using FSH.Framework.Core.Exceptions;
using FSH.Framework.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Framework.Infrastructure.Auth.OpenId;
public static class Extensions
{
    public static IServiceCollection AddOpenIdAuthentication(this IServiceCollection services, IConfiguration config)
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
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                RequireAudience = true,
                ValidateAudience = true
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

        return services;
    }
}
