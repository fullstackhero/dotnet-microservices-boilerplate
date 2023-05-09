using FSH.Framework.Infrastructure.Controllers;

namespace FluentPos.Cart.Api.Controllers;

public class CartsController : BaseApiController
{
    //private readonly ILogger<CartsController> _logger;

    //public CartsController(ILogger<CartsController> logger)
    //{
    //    _logger = logger;
    //}

    //[HttpGet("/{id:guid}", Name = nameof(GetCartAsync))]
    //[Authorize("cart:read")]
    //[ProducesResponseType((int)HttpStatusCode.OK)]
    //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
    //[ProducesResponseType(200, Type = typeof(CustomerCart))]
    //public async Task<IActionResult> GetCartAsync(Guid id)
    //{
    //    var query = new GetCart.Query(id);
    //    var response = await Mediator.Send(query);

    //    return Ok(response);
    //}

    //[HttpPut("/{id:guid}", Name = nameof(UpdateCartAsync))]
    //[Authorize("cart:write")]
    //[ProducesResponseType((int)HttpStatusCode.OK)]
    //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
    //public async Task<IActionResult> UpdateCartAsync(Guid id, UpdateCartRequestDto updateRequest)
    //{
    //    var command = new UpdateCart.Command(updateRequest, id);
    //    var response = await Mediator.Send(command);

    //    return Ok(response);
    //}

    //[HttpPost("/{id:guid}/checkout", Name = nameof(CheckoutCartAsync))]
    //[Authorize("cart:write")]
    //[ProducesResponseType((int)HttpStatusCode.Accepted)]
    //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
    //public async Task<IActionResult> CheckoutCartAsync(Guid id, CheckoutCartRequestDto checkoutRequest)
    //{
    //    var command = new CheckoutCart.Command(checkoutRequest, id);
    //    await Mediator.Send(command);

    //    return Ok();
    //}
}
