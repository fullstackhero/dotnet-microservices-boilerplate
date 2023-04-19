using FSH.Microservices.Infrastructure;
using FSH.Microservices.Infrastructure.Auth.OpenIddict;
using FSH.Microservices.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddExceptionMiddleware();
builder.Services.AddControllers();
builder.AddInfrastructure();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenIddictValidation(builder.Configuration);
var app = builder.Build();
app.UseInfrastructure(builder.Configuration, builder.Environment);
app.UseExceptionMiddleware();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthorization();
app.MapGet("/api", [Authorize] (ClaimsPrincipal user) => $"{user.Identity!.Name} is allowed to access Host.");
app.MapControllers();
app.Run();
