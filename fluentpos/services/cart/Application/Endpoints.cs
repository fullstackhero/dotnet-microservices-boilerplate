using FluentPos.Cart.Application.Dtos;
using FluentPos.Cart.Application.Features;
using FluentPos.Cart.Domain;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FluentPos.Cart.Application;
public static class Endpoints
{
    public static void MapCartEnpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", () => "Hello!")
        .AllowAnonymous()
        .Produces(200);

        // Get Customer Cart Details
        builder.MapGet("/{id:guid}", async (Guid id, ISender _mediatr) =>
        {
            var query = new GetCart.Query(id);
            return Results.Ok(await _mediatr.Send(query));
        })
        .RequireAuthorization("cart:read")
        .Produces(200, responseType: typeof(CustomerCart))
        .Produces(400);

        // Update Customer Cart
        builder.MapPut("/{id:guid}", async (Guid id, UpdateCartRequestDto updateRequest, ISender _mediatr) =>
        {
            var command = new UpdateCart.Command(updateRequest, id);
            return Results.Ok(await _mediatr.Send(command));
        })
        .RequireAuthorization("cart:write")
        .Produces(200)
        .Produces(400);

        // Checkout Customer Cart
        builder.MapPost("/{id:guid}/checkout", async (Guid id, CheckoutCartRequestDto checkoutRequest, ISender _mediatr) =>
        {
            var command = new CheckoutCart.Command(checkoutRequest, id);
            await _mediatr.Send(command);
            return Results.Ok();
        })
        .RequireAuthorization("cart:write")
        .Produces(202)
        .Produces(400);
    }
}
