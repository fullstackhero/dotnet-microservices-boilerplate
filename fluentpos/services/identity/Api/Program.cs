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
builder.AddInfrastructure(applicationAssembly: coreAssembly, enableSwagger: enableSwagger);
builder.ConfigureAuthServer<AppDbContext>(dbContextAssembly);
builder.Services.AddHostedService<SeedClientsAndScopes>();
var app = builder.Build();
app.UseInfrastructure(builder.Environment, enableSwagger);
app.Run();
