using System.Net;
using Catalog.Application.Products;
using FSH.Core.Web;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Catalog.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : BaseController
{
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [SwaggerOperation(Summary = "creates a new product and returns id.", Description = "creates a new product and returns id.")]
    public async Task<IActionResult> CreateAsync(CreateProduct.Request request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return Created(nameof(CreateAsync), result);
    }
}
