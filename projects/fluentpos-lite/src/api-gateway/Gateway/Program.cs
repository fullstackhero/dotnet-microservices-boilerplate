using FSH.Infrastructure;
using FSH.Infrastructure.Authentication;
using FSH.Infrastructure.Logging.Serilog;
using Microsoft.AspNetCore.Authentication;
var builder = WebApplication.CreateBuilder(args);
builder.RegisterSerilog();
builder.Services.RegisterJWTAuthentication();
builder.Services.AddInfrastructureServices();
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
var app = builder.Build();
app.MapGet("/", () => "Hello From Gateway");
app.UseStaticFiles(); // The middleware runs before routing happens => no route was found
app.UseAuthentication();
app.UseAuthorization();
app.ConfigureSerilog();
app.UseRouting();
app.MapReverseProxy(config =>
{
    config.Use(async (context, next) =>
    {
        var token = await context.GetTokenAsync("access_token");
        context.Request.Headers["Authorization"] = $"Bearer {token}";

        await next().ConfigureAwait(false);
    });
});

app.Run();
