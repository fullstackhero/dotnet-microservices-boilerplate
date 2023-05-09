using FluentPos.Cart.Core;
using FSH.Framework.Infrastructure;
using FSH.Framework.Infrastructure.Auth.OpenId;

var coreAssembly = typeof(CartCore).Assembly;
var builder = WebApplication.CreateBuilder(args);

var policyNames = new List<string> { "cart:read", "cart:write" };
builder.Services.AddOpenIdAuth(builder.Configuration, policyNames);
builder.AddInfrastructure(coreAssembly);
var app = builder.Build();
app.UseInfrastructure(builder.Configuration, builder.Environment);
app.Run();