using System.Reflection;
using Figgle;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;

namespace FSH.Infrastructure.Logging.Serilog;

public static class Extensions
{
    public static IApplicationBuilder ConfigureSerilog(this IApplicationBuilder app)
    {
        // app.UseSerilogRequestLogging();
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }
    public static string RegisterSerilog(this WebApplicationBuilder builder)
    {
        string appName = builder.Configuration.GetValue<string>("App:Name") ?? Assembly.GetEntryAssembly().GetName().Name;
        var elasticSearchUrl = builder.Configuration["ELKOptions:Uri"];
        var isDevelopmentEnvironment = builder.Environment.IsDevelopment();
        _ = builder.Host.UseSerilog((_, _, loggerConfig) =>
        {
            loggerConfig
                .ReadFrom.Configuration(builder.Configuration, "Logging")
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", appName)
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .Enrich.FromLogContext().WriteTo.Console();

            if (isDevelopmentEnvironment)
            {
                loggerConfig.WriteTo.File(new CompactJsonFormatter(), "Logs/logs.json",
                restrictedToMinimumLevel: LogEventLevel.Information,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 5);
            }

            if (!string.IsNullOrEmpty(elasticSearchUrl))
            {
                var formattedAppName = appName?.ToLower().Replace(".", "-").Replace(" ", "-");
                var indexFormat = $"{formattedAppName}-logs-{builder.Environment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}";
                _ = loggerConfig.WriteTo.Async(writeTo =>
                            writeTo.Elasticsearch(new(new Uri(elasticSearchUrl))
                            {
                                AutoRegisterTemplate = true,
                                IndexFormat = indexFormat,
                                MinimumLogEventLevel = LogEventLevel.Information,
                            })).Enrich.WithProperty("Environment", builder.Environment.EnvironmentName!);
            }

            loggerConfig
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error);
        });

        Console.WriteLine(FiggleFonts.Standard.Render(appName!));
        builder.Services.AddScoped<ExceptionMiddleware>();

        return appName;
    }
}
