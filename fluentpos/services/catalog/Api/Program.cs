using Dapr;
using FluentPos.Catalog.Core;
using FluentPos.Shared.Events.Catalog;
using FSH.Framework.Infrastructure;
using FSH.Framework.Infrastructure.Auth.OpenIddict;
using FSH.Framework.Infrastructure.Dapr;
using FSH.Framework.Persistence.NoSQL.Mongo;

var coreAssembly = typeof(CatalogCore).Assembly;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCoreCatalogService();
builder.Services.AddMongoDbContext<MongoDbContext>(builder.Configuration);
builder.AddInfrastructure(coreAssembly);
builder.Services.AddOpenIddictValidation(builder.Configuration);
var app = builder.Build();
app.UseInfrastructure(builder.Configuration, builder.Environment);

app.MapPost("/test", [Topic(DaprConstants.RMQPubSub, nameof(ProductCreatedEvent))] (ProductCreatedEvent item) =>
{
    Console.WriteLine($"Received Message \n Product Id : {item.ProductId} \n ProductName: {item.ProductName}");
    return Results.Ok();
});


app.Run();