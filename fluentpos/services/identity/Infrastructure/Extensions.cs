using FluentPos.Identity.Application;
using FluentPos.Identity.Domain.Users;
using FluentPos.Identity.Infrastructure.Persistence;
using FSH.Framework.Infrastructure;
using FSH.Framework.Infrastructure.Auth.OpenIddict;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace FluentPos.Identity.Infrastructure;
public static class Extensions
{
    internal static bool enableSwagger = false;
    public static void AddIdentityInfrastructure(this WebApplicationBuilder builder)
    {
        var coreAssembly = typeof(IdentityCore).Assembly;
        var dbContextAssembly = typeof(AppDbContext).Assembly;

        builder.Services.AddIdentityExtensions();
        builder.AddInfrastructure(applicationAssembly: coreAssembly, enableSwagger: enableSwagger);
        builder.ConfigureAuthServer<AppDbContext>(dbContextAssembly);
        builder.Services.AddHostedService<SeedClientsAndScopes>();
    }

    public static void UseIdentityInfrastructure(this WebApplication app)
    {
        app.UseInfrastructure(app.Environment, enableSwagger);
    }
    internal static IServiceCollection AddIdentityExtensions(this IServiceCollection services)
    {
        services
            .AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            // Configure Identity to use the same JWT claims as OpenIddict instead
            // of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            options.ClaimsIdentity.UserNameClaimType = Claims.Name;
            options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
            options.ClaimsIdentity.RoleClaimType = Claims.Role;
            options.ClaimsIdentity.EmailClaimType = Claims.Email;

            // Note: to require account confirmation before login,
            // register an email sender service (IEmailSender) and
            // set options.SignIn.RequireConfirmedAccount to true.
            //
            // For more information, visit https://aka.ms/aspaccountconf.
            options.SignIn.RequireConfirmedAccount = false;
        });

        return services;
    }
}
