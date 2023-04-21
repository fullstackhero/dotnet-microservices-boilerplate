using FluentPos.Catalog.Core.Products.Dtos;
using FluentPos.Catalog.Core.Products.Features;
using FSH.Microservices.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers;
[Route("api/[controller]")]
[ApiController]

[Authorize]
public class ProductsController : BaseApiController
{
    [HttpPost(Name = "AddProductAsync")]
    public async Task<IActionResult> AddProductAsync(AddProductDto request)
    {
        var command = new AddProduct.Command(request);
        var commandResponse = await Mediator.Send(command);

        return CreatedAtRoute("AddProductAsync", new { commandResponse.Id });
    }

    [HttpGet("{id:guid}", Name = "GetProductAsync")]
    public async Task<IActionResult> GetProductAsync(Guid id)
    {
        var query = new GetProduct.Query(id);
        var queryResponse = await Mediator.Send(query);

        return Ok(queryResponse);
    }

    [HttpGet(Name = "GetProductsAsync")]
    public async Task<IActionResult> GetProductsAsync([FromQuery] ProductsParametersDto parameters)
    {
        var query = new GetProducts.Query(parameters);
        var queryResponse = await Mediator.Send(query);

        return Ok(queryResponse);
    }
}
