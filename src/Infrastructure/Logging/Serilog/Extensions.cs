using FSH.Microservices.Infrastructure.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;

namespace FSH.Microservices.Infrastructure.Logging.Serilog;

public static class Extensions
{
    public static void ConfigureSerilog(this WebApplicationBuilder builder, IConfiguration config)
    {
        var serilogOptions = builder.Services.AddLoadValidateOptions<SerilogOptions>(config);
        _ = builder.Host.UseSerilog((_, sp, serilogConfig) =>
        {
            string appName = serilogOptions.AppName;
            string elasticSearchUrl = serilogOptions.ElasticSearchUrl;
            bool writeToFile = serilogOptions.WriteToFile;
            bool structuredConsoleLogging = serilogOptions.StructuredConsoleLogging;
            string minLogLevel = serilogOptions.MinimumLogLevel.ToLower();
            if (!serilogOptions.MinimalLogging)
            {
                ConfigureEnrichers(serilogConfig, appName);
            }
            ConfigureConsoleLogging(serilogConfig, structuredConsoleLogging);
            ConfigureWriteToFile(serilogConfig, writeToFile);
            //ConfigureElasticSearch(builder, serilogConfig, appName, elasticSearchUrl);
            SetMinimumLogLevel(serilogConfig, minLogLevel);
            OverideMinimumLogLevel(serilogConfig);
        });
    }

    private static void ConfigureEnrichers(LoggerConfiguration config, string appName)
    {
        config
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", appName)
            .Enrich.WithExceptionDetails()
            .Enrich.WithMachineName()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId();
    }

    private static void ConfigureConsoleLogging(LoggerConfiguration serilogConfig, bool structuredConsoleLogging)
    {
        if (structuredConsoleLogging)
        {
            serilogConfig.WriteTo.Async(wt => wt.Console(new CompactJsonFormatter()));
        }
        else
        {
            serilogConfig.WriteTo.Async(wt => wt.Console());
        }
    }

    private static void ConfigureWriteToFile(LoggerConfiguration serilogConfig, bool writeToFile)
    {
        if (writeToFile)
        {
            serilogConfig.WriteTo.File(
             new CompactJsonFormatter(),
             "Logs/logs.json",
             restrictedToMinimumLevel: LogEventLevel.Information,
             rollingInterval: RollingInterval.Day,
             retainedFileCountLimit: 5);
        }
    }

    private static void SetMinimumLogLevel(LoggerConfiguration serilogConfig, string minLogLevel)
    {
        var loggingLevelSwitch = new LoggingLevelSwitch();
        loggingLevelSwitch.MinimumLevel = minLogLevel.ToLower() switch
        {
            "debug" => LogEventLevel.Debug,
            "information" => LogEventLevel.Information,
            "warning" => LogEventLevel.Warning,
            _ => LogEventLevel.Information,
        };
        serilogConfig.MinimumLevel.ControlledBy(loggingLevelSwitch);
    }

    private static void OverideMinimumLogLevel(LoggerConfiguration serilogConfig)
    {
        serilogConfig
                     .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                     .MinimumLevel.Override("Hangfire", LogEventLevel.Warning)
                     .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                     .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error);
    }
}
