using FluentPos.Cart.Application;
using FluentPos.Cart.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.AddCartInfrastructure();

var app = builder.Build();
app.UseCartInfrastructure();
app.MapCartEnpoints();
app.Run();