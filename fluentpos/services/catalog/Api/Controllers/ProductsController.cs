using FluentPos.Catalog.Application.Products.Dtos;
using FluentPos.Catalog.Application.Products.Features;
using FSH.Framework.Core.Pagination;
using FSH.Framework.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluentPos.Catalog.Api.Controllers;

public class ProductsController : BaseApiController
{
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ILogger<ProductsController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = nameof(AddProductAsync))]
    [Authorize("catalog:write")]
    [ProducesResponseType(201, Type = typeof(ProductDto))]
    public async Task<IActionResult> AddProductAsync(AddProductDto request)
    {
        var command = new AddProduct.Command(request);
        var commandResponse = await Mediator.Send(command);

        return CreatedAtRoute(nameof(GetProductAsync), new { commandResponse.Id }, commandResponse);
    }

    [HttpGet("{id:guid}", Name = nameof(GetProductAsync))]
    [Authorize("catalog:read")]
    [ProducesResponseType(200, Type = typeof(ProductDetailsDto))]
    public async Task<IActionResult> GetProductAsync(Guid id)
    {
        var query = new GetProductDetails.Query(id);
        var queryResponse = await Mediator.Send(query);

        return Ok(queryResponse);
    }

    [HttpGet(Name = nameof(GetProductsAsync))]
    [Authorize("catalog:read")]
    [ProducesResponseType(200, Type = typeof(PagedList<ProductDto>))]
    public async Task<IActionResult> GetProductsAsync([FromQuery] ProductsParametersDto parameters)
    {
        var query = new GetProducts.Query(parameters);
        var queryResponse = await Mediator.Send(query);

        return Ok(queryResponse);
    }

    [HttpDelete("{id:guid}", Name = nameof(DeleteProductsAsync))]
    [Authorize("catalog:write")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteProductsAsync(Guid id)
    {
        var command = new DeleteProduct.Command(id);
        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("{id:guid}", Name = nameof(UpdateProductsAsync))]
    [Authorize("catalog:write")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateProductsAsync(Guid id, UpdateProductDto updateProductDto)
    {
        var command = new UpdateProduct.Command(updateProductDto, id);
        await Mediator.Send(command);

        return NoContent();
    }
}
