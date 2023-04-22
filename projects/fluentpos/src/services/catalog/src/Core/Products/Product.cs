using FluentPos.Catalog.Core.Products.Dtos;
using FSH.Microservices.Core.Domain;
using System.Text.RegularExpressions;

namespace FluentPos.Catalog.Core.Products;

public class Product : BaseEntity
{
    public string Name { get; private set; } = default!;
    public string Details { get; private set; } = default!;
    public string Code { get; private set; } = default!;
    public string Slug { get; private set; } = default!;
    public bool Active { get; private set; } = true;
    public decimal Cost { get; private set; }
    public decimal Price { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal AlertQuantity { get; private set; }
    public bool TrackQuantity { get; private set; }

    public Product Update(UpdateProductDto productDto)
    {
        if (productDto.Name is not null && Name?.Equals(productDto.Name) is not true) Name = productDto.Name;
        if (productDto.Details is not null && Details?.Equals(productDto.Details) is not true) Details = productDto.Details;
        if (productDto.Price.HasValue && Price != productDto.Price.Value) Price = productDto.Price.Value;
        if (productDto.Cost.HasValue && Cost != productDto.Cost.Value) Cost = productDto.Cost.Value;
        if (productDto.TrackQuantity.HasValue && TrackQuantity != productDto.TrackQuantity) TrackQuantity = productDto.TrackQuantity.Value;
        if (productDto.AlertQuantity.HasValue && AlertQuantity != productDto.AlertQuantity.Value) AlertQuantity = productDto.AlertQuantity.Value;
        if (productDto.Quantity.HasValue && Quantity != productDto.Quantity.Value) Quantity = productDto.Quantity.Value;
        return this;
    }
    public static Product Create(AddProductDto productDto)
    {
        Product product = new()
        {
            Name = productDto.Name!,
            Details = productDto.Details!,
            Code = productDto.Code!,
            Slug = GetProductSlug(productDto.Name!),
            Cost = productDto.Cost,
            AlertQuantity = productDto.AlertQuantity,
            TrackQuantity = productDto.TrackQuantity,
            Quantity = productDto.Quantity,
            Active = true,
            Price = productDto.Price
        };

        //product.AddDomainEvent(new ProductCreatedEvent
        //{
        //    Id = product.Id,
        //    Name = product.Name,
        //    Quantity = product.Quantity,
        //    Price = product.Price
        //});

        return product;
    }

    private static string GetProductSlug(string name)
    {
        name = name.Trim();
        name = name.ToLower();
        name = Regex.Replace(name, "[^a-z0-9]+", "-");
        name = Regex.Replace(name, "--+", "-");
        name = name.Trim('-');
        return name;
    }

    public static string GetCacheKey(Guid id)
    {
        return $"Product:{id}";
    }
}
