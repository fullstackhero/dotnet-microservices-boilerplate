using FSH.Microservices.Infrastructure.Logging.Serilog;
using FSH.Microservices.Infrastructure.Options;
using FSH.Microservices.Infrastructure.Services;
using FSH.Microservices.Infrastructure.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Microservices.Infrastructure;

public static class Extensions
{
    public static void AddInfrastructure(this WebApplicationBuilder builder, bool enableSwagger = true)
    {
        var config = builder.Configuration;
        var appOptions = builder.Services.ValidateAndLoad<AppOptions>(config);
        builder.ConfigureSerilog(appOptions.Name);
        builder.Services.AddProblemDetails();
        if (enableSwagger) builder.Services.AddSwaggerExtension(builder.Configuration, appOptions.Name);
        builder.Services.AddInternalServices();
    }

    public static void UseInfrastructure(this IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment env, bool enableSwagger = true)
    {
        if (enableSwagger) app.UseSwaggerExtension(configuration, env);
    }
}
