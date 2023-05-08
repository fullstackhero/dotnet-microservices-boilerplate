using FluentPos.Cart.Core.Carts;
using FluentPos.Cart.Core.Carts.Dtos;
using FluentPos.Cart.Core.Carts.Features;
using FSH.Framework.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluentPos.Cart.Api.Controllers;

public class CartsController : BaseApiController
{
    private readonly ILogger<CartsController> _logger;

    public CartsController(ILogger<CartsController> logger)
    {
        _logger = logger;
    }

    [HttpGet("/{id:guid}", Name = nameof(GetCartAsync))]
    [Authorize("cart:read")]
    [ProducesResponseType(200, Type = typeof(CustomerCart))]
    public async Task<IActionResult> GetCartAsync(Guid id)
    {
        var query = new GetCart.Query(id);
        var queryResponse = await Mediator.Send(query);

        return Ok(queryResponse);
    }

    [HttpPut("/{id:guid}", Name = nameof(UpdateCartAsync))]
    [Authorize("cart:write")]
    [ProducesResponseType(201, Type = typeof(CustomerCart))]
    public async Task<IActionResult> UpdateCartAsync(Guid id, UpdateCartDto updateCartDto)
    {
        var command = new UpdateCart.Command(updateCartDto, id);
        var response = await Mediator.Send(command);

        return Ok(response);
    }
}
