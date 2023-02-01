using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duende.IdentityServer.EntityFramework.Entities;
using FSH.Core.Common;
using FSH.Core.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Infrastructure.Authentication;

public static class Extensions
{
    public static IServiceCollection RegisterJWTAuthentication(this IServiceCollection services)
    {
        var authConfig = services.GetRequiredConfiguration<AuthOptions>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = authConfig.Authority;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters.ValidateAudience = false;
            });

        if (!string.IsNullOrEmpty(authConfig.Audience))
        {
            services.AddAuthorization(options =>
                options.AddPolicy(nameof(ApiScope), policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", authConfig.Audience);
                })
            );
        }

        services.AddHttpContextAccessor();
        services.AddScoped<IAuthenticatedUser, AuthenticatedUser>();

        return services;
    }
}
