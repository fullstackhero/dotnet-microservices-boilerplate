using FSH.Framework.Infrastructure.Options;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;

namespace FSH.Framework.Infrastructure.Logging.Serilog;

public static class Extensions
{
    public static void ConfigureSerilog(this WebApplicationBuilder builder, string appName)
    {
        var config = builder.Configuration;
        var serilogOptions = builder.Services.BindValidateReturn<SerilogOptions>(config);
        _ = builder.Host.UseSerilog((_, _, serilogConfig) =>
        {
            if (serilogOptions.EnableErichers) ConfigureEnrichers(serilogConfig, appName);
            ConfigureConsoleLogging(serilogConfig, serilogOptions.StructuredConsoleLogging);
            ConfigureWriteToFile(serilogConfig, serilogOptions.WriteToFile, serilogOptions.RetentionFileCount, appName);
            SetMinimumLogLevel(serilogConfig, serilogOptions.MinimumLogLevel);
            if (serilogOptions.OverideMinimumLogLevel) OverideMinimumLogLevel(serilogConfig);
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

    private static void ConfigureWriteToFile(LoggerConfiguration serilogConfig, bool writeToFile, int retainedFileCount, string appName)
    {
        if (writeToFile)
        {
            serilogConfig.WriteTo.File(
             new CompactJsonFormatter(),
             $"Logs/{appName.ToLower()}.logs.json",
             restrictedToMinimumLevel: LogEventLevel.Information,
             rollingInterval: RollingInterval.Day,
             retainedFileCountLimit: retainedFileCount);
        }
    }

    private static void SetMinimumLogLevel(LoggerConfiguration serilogConfig, string minLogLevel)
    {
        var loggingLevelSwitch = new LoggingLevelSwitch
        {
            MinimumLevel = minLogLevel.ToLower() switch
            {
                "debug" => LogEventLevel.Debug,
                "information" => LogEventLevel.Information,
                "warning" => LogEventLevel.Warning,
                _ => LogEventLevel.Information,
            }
        };
        serilogConfig.MinimumLevel.ControlledBy(loggingLevelSwitch);
    }

    private static void OverideMinimumLogLevel(LoggerConfiguration serilogConfig)
    {
        serilogConfig
                     .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                     .MinimumLevel.Override("Hangfire", LogEventLevel.Warning)
                     .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                     .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
                     .MinimumLevel.Override("OpenIddict.Validation", LogEventLevel.Error)
                     .MinimumLevel.Override("System.Net.Http.HttpClient.OpenIddict", LogEventLevel.Error);
    }
}
