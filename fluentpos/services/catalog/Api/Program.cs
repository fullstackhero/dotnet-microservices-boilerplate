using FluentPos.Catalog.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.AddCatalogInfrastructure();
var app = builder.Build();
app.UseCatalogInfrastructure();

app.MapGet("/", () => "Hello From Catalog Service").AllowAnonymous();
app.Run();