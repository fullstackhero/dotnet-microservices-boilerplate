using FluentPOS.Auth.Api.Database;
using FSH.Microservices.Infrastructure;
using FSH.Microservices.Infrastructure.Auth.OpenIddict;
using FSH.Microservices.Infrastructure.Middlewares;

bool enableSwagger = false;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();


builder.AddInfrastructure(enableSwagger: enableSwagger);


builder.ConfigureOpenIddictServer<IdentityDbContext>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHostedService<SeedClientsAndScopes>();
}

builder.Services.AddExceptionMiddleware();
var app = builder.Build();
app.UseInfrastructure(builder.Configuration, builder.Environment, enableSwagger);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseExceptionMiddleware();
app.Run();
