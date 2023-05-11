using Dapr;
using FluentPos.Catalog.Core;
using FluentPos.Shared.Events;
using FSH.Framework.Infrastructure;
using FSH.Framework.Infrastructure.Auth.OpenId;
using FSH.Framework.Infrastructure.Dapr;
using FSH.Framework.Persistence.NoSQL.Mongo;

var coreAssembly = typeof(CatalogCore).Assembly;
var builder = WebApplication.CreateBuilder(args);

var policyNames = new List<string> { "catalog:read", "catalog:write" };
builder.Services.AddOpenIdAuth(builder.Configuration, policyNames);

builder.Services.AddCoreCatalogService();
builder.Services.AddMongoDbContext<MongoDbContext>(builder.Configuration);
builder.AddInfrastructure(coreAssembly);
var app = builder.Build();
app.UseInfrastructure(builder.Environment);

// this will be invoked when a new product is created
// this will be handled in some other way
// keeping it here to demonstrate cross-container/cross-service communication via RMQ topics
app.MapPost("/handleProductCreation", [Topic(DaprConstants.RMQPubSub, nameof(ProductCreatedEvent))] (ProductCreatedEvent item) =>
{
    Console.WriteLine($"Received ProductCreatedEvent Notification - Product Id : {item.ProductId} & ProductName: {item.ProductName}");
    return Results.Ok();
});

// this will be invoked when a cart is checked out using the cart/{customerId}/checkout [POST] endpoint.
// this will be moved to order api later on
app.MapPost("/handleCheckout", [Topic(DaprConstants.RMQPubSub, nameof(CartCheckedOutEvent))] (CartCheckedOutEvent @event) =>
{
    Console.WriteLine($"Received CartCheckedOutEvent Notification - Customer Id : {@event.CustomerId} & Credit Card Number: {@event.CreditCardNumber}");
    return Results.Ok();
});

app.MapGet("/", () => "Hello From Catalog Service").AllowAnonymous();
app.Run();