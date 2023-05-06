using FSH.Framework.Infrastructure;
using FSH.Framework.Infrastructure.Auth.OpenId;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);
bool enableSwagger = false;
builder.Services.AddOpenIdAuthentication(builder.Configuration);
builder.AddInfrastructure(enableSwagger: enableSwagger);
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
var app = builder.Build();
app.UseInfrastructure(builder.Configuration, builder.Environment, enableSwagger: enableSwagger);
app.MapGet("/", () => "Hello From Gateway");
app.UseRouting();
app.MapReverseProxy(config =>
{
    config.Use(async (context, next) =>
    {
        string? token = await context.GetTokenAsync("access_token");
        context.Request.Headers["Authorization"] = $"Bearer {token}";

        await next().ConfigureAwait(false);
    });
});

app.Run();