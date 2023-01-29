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

    [HttpGet("{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [SwaggerOperation(Summary = "gets product by id.", Description = "gets product by id.")]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProductById.Request(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [SwaggerOperation(Summary = "get products.", Description = "get products.")]
    public async Task<IActionResult> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProducts.Request(pageNumber, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [SwaggerOperation(Summary = "delete product by id.", Description = "delete product by id.")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteProduct.Request(id), cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [SwaggerOperation(Summary = "update product by id.", Description = "update product by id.")]
    public async Task<IActionResult> UpdateAsync(UpdateProduct.Request request, Guid id, CancellationToken cancellationToken)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request, cancellationToken));
    }
}
