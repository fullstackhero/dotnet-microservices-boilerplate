using FluentPos.Catalog.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.AddCatalogInfrastructure();
var app = builder.Build();
app.UseCatalogInfrastructure();

//// this will be invoked when a new product is created
//// this will be handled in some other way
//// keeping it here to demonstrate cross-container/cross-service communication via RMQ topics
//app.MapPost("/handleProductCreation", [Topic(DaprConstants.RMQPubSub, nameof(ProductCreatedEvent))] (ProductCreatedEvent item) =>
//{
//    Console.WriteLine($"Received ProductCreatedEvent Notification - Product Id : {item.ProductId} & ProductName: {item.ProductName}");
//    return Results.Ok();
//});

//// this will be invoked when a cart is checked out using the cart/{customerId}/checkout [POST] endpoint.
//// this will be moved to order api later on
//app.MapPost("/handleCheckout", [Topic(DaprConstants.RMQPubSub, nameof(CartCheckedOutEvent))] (CartCheckedOutEvent @event) =>
//{
//    Console.WriteLine($"Received CartCheckedOutEvent Notification - Customer Id : {@event.CustomerId} & Credit Card Number: {@event.CreditCardNumber}");
//    return Results.Ok();
//});

app.MapGet("/", () => "Hello From Catalog Service").AllowAnonymous();
app.Run();