using FSH.Microservices.Infrastructure;
using FSH.Microservices.Infrastructure.Auth.OpenIddict;
using FSH.Microservices.Infrastructure.Logging.Serilog;
using FSH.Microservices.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddExceptionMiddleware();
builder.Services.AddControllers();
builder.AddSerilogConfiguration(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterOIDAuthValidation(builder.Configuration);
var app = builder.Build();
app.UseExceptionMiddleware();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthorization();
app.MapGet("/api", [Authorize] (ClaimsPrincipal user) => $"{user.Identity!.Name} is allowed to access Host.");
app.MapControllers();
app.Run();
