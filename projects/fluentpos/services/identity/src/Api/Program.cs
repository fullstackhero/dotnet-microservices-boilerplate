using FluentPOS.Identity.Core;
using FluentPOS.Identity.Infrastructure;
using FluentPOS.Identity.Infrastructure.Persistence;
using FSH.Microservices.Infrastructure;
using FSH.Microservices.Infrastructure.Auth.OpenIddict;

bool enableSwagger = true;
var coreAssembly = typeof(IdentityCore).Assembly;
var dbContextAssembly = typeof(AppDbContext).Assembly;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddIdentityExtensions();
builder.AddInfrastructure(enableSwagger: enableSwagger, coreAssembly: coreAssembly);
builder.ConfigureOpenIddictServer<AppDbContext>(dbContextAssembly);
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHostedService<SeedClientsAndScopes>();
}

var app = builder.Build();
app.UseInfrastructure(builder.Configuration, builder.Environment, enableSwagger);
app.Run();
