using FluentPos.Catalog.Core;
using FSH.Microservices.Infrastructure;
using FSH.Microservices.Infrastructure.Auth.OpenIddict;
using FSH.Microservices.Infrastructure.Middlewares;
using FSH.Microservices.Persistence.NoSQL.Mongo;

var coreAssembly = typeof(CatalogCore).Assembly;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCoreCatalogService();
builder.Services.AddExceptionMiddleware();
builder.Services.AddControllers();
builder.Services.AddMongoDbContext<MongoDbContext>(builder.Configuration);
builder.AddInfrastructure(coreAssembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenIddictValidation(builder.Configuration);
var app = builder.Build();
app.UseInfrastructure(builder.Configuration, builder.Environment);
app.UseExceptionMiddleware();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
