using Figgle;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Exceptions;
using System.Reflection;

namespace FSH.Infrastructure.Logging.Serilog;

public static class Extensions
{
    public static IApplicationBuilder ConfigureSerilog(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();
        return app;
    }
    public static string RegisterSerilog(this WebApplicationBuilder builder)
    {
        string appName = builder.Configuration.GetValue<string>("App:Name") ?? Assembly.GetEntryAssembly().GetName().Name;
        _ = builder.Host.UseSerilog((_, _, loggerConfig) =>
        {
            loggerConfig
                .ReadFrom.Configuration(builder.Configuration, "Logging")
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", appName)
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .Enrich.FromLogContext().WriteTo.Console();
        });

        Console.WriteLine(FiggleFonts.Standard.Render(appName!));

        return appName;
    }
}
