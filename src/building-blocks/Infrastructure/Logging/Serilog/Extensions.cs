using Figgle;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace FSH.Infrastructure.Logging.Serilog;

public static class Extensions
{
    public static IApplicationBuilder ConfigureSerilog(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();
        return app;
    }
    public static WebApplicationBuilder RegisterSerilog(this WebApplicationBuilder builder)
    {
        string appName = builder.Configuration.GetValue<string>("App:Name") ?? "default-app";
        _ = builder.Host.UseSerilog((_, _, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(builder.Configuration, "Logging")
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", appName)
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .Enrich.FromLogContext();
            loggerConfiguration.WriteTo.Console();
        });

        Console.WriteLine(FiggleFonts.Standard.Render(appName!));

        return builder;
    }
}
