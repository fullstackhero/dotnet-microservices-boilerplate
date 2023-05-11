using System.Reflection;
using FluentValidation;
using FSH.Framework.Infrastructure.Behaviors;
using FSH.Framework.Infrastructure.Caching;
using FSH.Framework.Infrastructure.Dapr;
using FSH.Framework.Infrastructure.Logging.Serilog;
using FSH.Framework.Infrastructure.Mapping.Mapster;
using FSH.Framework.Infrastructure.Middlewares;
using FSH.Framework.Infrastructure.Options;
using FSH.Framework.Infrastructure.Services;
using FSH.Framework.Infrastructure.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Framework.Infrastructure;

public static class Extensions
{
    public const string AllowAllOrigins = "AllowAll";
    public static void AddInfrastructure(this WebApplicationBuilder builder, Assembly? coreAssembly = null, bool enableSwagger = true)
    {
        var config = builder.Configuration;
        var appOptions = builder.Services.BindValidateReturn<AppOptions>(config);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: AllowAllOrigins,
                              builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });

        builder.Services.AddExceptionMiddleware();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.ConfigureSerilog(appOptions.Name);
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddDaprBuildingBlocks();
        if (coreAssembly != null)
        {
            builder.Services.AddMapsterExtension(coreAssembly);
            builder.Services.AddBehaviors();
            builder.Services.AddValidatorsFromAssembly(coreAssembly);
            builder.Services.AddMediatR(o => o.RegisterServicesFromAssembly(coreAssembly));
        }

        if (enableSwagger) builder.Services.AddSwaggerExtension(config);
        builder.Services.AddCachingService();
        builder.Services.AddInternalServices();
    }

    public static void UseInfrastructure(this WebApplication app, IWebHostEnvironment env, bool enableSwagger = true)
    {
        //Preserve Order
        app.UseCors(AllowAllOrigins);
        app.UseExceptionMiddleware();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCloudEvents();
        app.MapSubscribeHandler();
        app.MapControllers();
        if (enableSwagger) app.UseSwaggerExtension(env);
    }
}
