using FSH.Framework.Infrastructure;
using FSH.Framework.Infrastructure.Auth.OpenIddict;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);
bool enableSwagger = false;
builder.AddInfrastructure(enableSwagger: enableSwagger);
builder.Services.AddOpenIddictValidation(builder.Configuration);
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
var app = builder.Build();
app.UseInfrastructure(builder.Configuration, builder.Environment, enableSwagger: enableSwagger);
app.MapGet("/", () => "Hello From Gateway");
app.UseStaticFiles(); // The middleware runs before routing happens => no route was found
app.UseAuthentication();
app.UseAuthorization();
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