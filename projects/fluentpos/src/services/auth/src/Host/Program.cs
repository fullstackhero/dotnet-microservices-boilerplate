using FluentPOS.Auth.Host.Database;
using FluentPOS.Auth.Host.Handlers;
using FSH.Microservices.Infrastructure;
using FSH.Microservices.Infrastructure.Logging.Serilog;
using FSH.Microservices.Infrastructure.Middlewares;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static OpenIddict.Server.OpenIddictServerEvents;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Assembly assembly = Assembly.GetExecutingAssembly();
builder.AddSerilogConfiguration(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    options.UseNpgsql(connectionString, m => m.MigrationsAssembly(assembly.FullName));
    options.UseOpenIddict();
});

builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore().UseDbContext<IdentityDbContext>();
    })
    .AddServer(options =>
    {
        options.SetAuthorizationEndpointUris("/connect/authorize")
               .SetIntrospectionEndpointUris("/connect/introspect")
               .SetTokenEndpointUris("/connect/token");
        options.AllowClientCredentialsFlow();
        if (builder.Environment.IsDevelopment())
        {
            // Disable Payload Encryption in JWTs
            options.DisableAccessTokenEncryption();
            options.AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate();
        }
        options.UseAspNetCore()
               .EnableTokenEndpointPassthrough();
        options.AddEventHandler<ApplyTokenResponseContext>(builder =>
        {
            builder.UseScopedHandler<TokenResponseHandler>();
        });
    });

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHostedService<SeedClientsAndScopes>();
}

builder.Services.AddExceptionMiddleware();
var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseExceptionMiddleware();
app.Run();
