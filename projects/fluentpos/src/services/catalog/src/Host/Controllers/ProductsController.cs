using FluentPos.Catalog.Core.Products.Dtos;
using FluentPos.Catalog.Core.Products.Features;
using FSH.Microservices.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController : BaseApiController
{
    [HttpPost(Name = "AddProductAsync")]
    [Authorize]
    public async Task<IActionResult> AddProductAsync(AddProductDto request)
    {
        var command = new AddProduct.Command(request);
        var commandResponse = await Mediator.Send(command);

        return CreatedAtRoute("AddProductAsync", new { commandResponse.Id }, commandResponse);
    }
}
