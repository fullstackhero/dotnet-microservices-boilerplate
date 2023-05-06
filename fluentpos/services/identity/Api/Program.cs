using FluentPos.Identity.Core;
using FluentPos.Identity.Infrastructure;
using FluentPos.Identity.Infrastructure.Persistence;
using FSH.Framework.Infrastructure;
using FSH.Framework.Infrastructure.Auth.OpenIddict;

bool enableSwagger = false;
var coreAssembly = typeof(IdentityCore).Assembly;
var dbContextAssembly = typeof(AppDbContext).Assembly;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddIdentityExtensions();
builder.AddInfrastructure(enableSwagger: enableSwagger, coreAssembly: coreAssembly);
builder.ConfigureAuthServer<AppDbContext>(dbContextAssembly);
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHostedService<SeedClientsAndScopes>();
}
var app = builder.Build();
app.UseInfrastructure(builder.Configuration, builder.Environment, enableSwagger);
app.Run();
