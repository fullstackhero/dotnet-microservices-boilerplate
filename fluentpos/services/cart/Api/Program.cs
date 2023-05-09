using FluentPos.Cart.Core;
using FluentPos.Cart.Core.Dtos;
using FluentPos.Cart.Core.Features;
using FSH.Framework.Infrastructure;
using FSH.Framework.Infrastructure.Auth.OpenId;
using MediatR;

var coreAssembly = typeof(CartCore).Assembly;
var builder = WebApplication.CreateBuilder(args);

var policyNames = new List<string> { "cart:read", "cart:write" };
builder.Services.AddOpenIdAuth(builder.Configuration, policyNames);
builder.AddInfrastructure(coreAssembly);
var app = builder.Build();
app.UseInfrastructure(builder.Configuration, builder.Environment);

// Get Customer Cart Details
app.MapGet("/{id:guid}", async (Guid id, ISender _mediatr) =>
{
    var query = new GetCart.Query(id);
    return Results.Ok(await _mediatr.Send(query));
})
.RequireAuthorization("cart:read")
.Produces(200, responseType: typeof(CustomerCart))
.Produces(400);


// Update Customer Cart
app.MapPut("/{id:guid}", async (Guid id, UpdateCartRequestDto updateRequest, ISender _mediatr) =>
{
    var command = new UpdateCart.Command(updateRequest, id);
    return Results.Ok(await _mediatr.Send(command));
})
.RequireAuthorization("cart:write")
.Produces(200)
.Produces(400);


// Checkout Customer Cart
app.MapPost("/{id:guid}/checkout", async (Guid id, CheckoutCartRequestDto checkoutRequest, ISender _mediatr) =>
{
    var command = new CheckoutCart.Command(checkoutRequest, id);
    await _mediatr.Send(command);
    return Results.Ok();
})
.RequireAuthorization("cart:write")
.Produces(202)
.Produces(400);

app.Run();