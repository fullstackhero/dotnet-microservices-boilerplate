using FluentPOS.Auth.Host.Database;
using FSH.Microservices.Infrastructure.Logging.Serilog;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Assembly assembly = Assembly.GetExecutingAssembly();
builder.Services.ConfigureSerilog();
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
        options.UseAspNetCore().EnableAuthorizationEndpointPassthrough().EnableTokenEndpointPassthrough();
    });

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHostedService<SeedClientsAndScopes>();
}
var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
