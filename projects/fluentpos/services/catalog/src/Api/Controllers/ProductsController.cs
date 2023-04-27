﻿using FluentPos.Catalog.Core.Products.Dtos;
using FluentPos.Catalog.Core.Products.Features;
using FSH.Microservices.Core.Pagination;
using FSH.Microservices.Infrastructure.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace FluentPos.Catalog.Api.Controllers;
[Route("api/[controller]")]
[ApiController]

public class ProductsController : BaseApiController
{
    [HttpPost(Name = "AddProductAsync")]
    [ProducesResponseType(201, Type = typeof(ProductDto))]
    public async Task<IActionResult> AddProductAsync(AddProductDto request)
    {
        var command = new AddProduct.Command(request);
        var commandResponse = await Mediator.Send(command);

        return CreatedAtRoute("AddProductAsync", commandResponse);
    }

    [HttpGet("{id:guid}", Name = "GetProductAsync")]
    [ProducesResponseType(200, Type = typeof(ProductDetailsDto))]
    public async Task<IActionResult> GetProductAsync(Guid id)
    {
        var query = new GetProductDetails.Query(id);
        var queryResponse = await Mediator.Send(query);

        return Ok(queryResponse);
    }

    [HttpGet(Name = "GetProductsAsync")]
    [ProducesResponseType(200, Type = typeof(PagedList<ProductDto>))]
    public async Task<IActionResult> GetProductsAsync([FromQuery] ProductsParametersDto parameters)
    {
        var query = new GetProducts.Query(parameters);
        var queryResponse = await Mediator.Send(query);

        return Ok(queryResponse);
    }

    [HttpDelete("{id:guid}", Name = "DeleteProductsAsync")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteProductsAsync(Guid id)
    {
        var command = new DeleteProduct.Command(id);
        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("{id:guid}", Name = "UpdateProductsAsync")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateProductsAsync(Guid id, UpdateProductDto updateProductDto)
    {
        var command = new UpdateProduct.Command(updateProductDto, id);
        await Mediator.Send(command);

        return NoContent();
    }
}