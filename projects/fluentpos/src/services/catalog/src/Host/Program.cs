using FSH.Microservices.Infrastructure.Auth.OpenIddict;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterOIDAuthValidation(builder.Configuration);
var app = builder.Build();
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
