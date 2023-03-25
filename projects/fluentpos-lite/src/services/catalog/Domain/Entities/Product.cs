using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentPOS.Lite.Events.Catalog;
using FSH.Core.Domain;

namespace FluentPOS.Lite.Catalog.Domain.Entities;

public class Product : AuditableEntity
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

    public Product Update(string? name, decimal? price)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (price.HasValue && Price != price) Price = price.Value;
        return this;
    }

    public static Product Create(string name, string details, string code, decimal cost, decimal price, decimal quantity, decimal alertQuantity = 10, bool trackQuantity = true)
    {
        return Create(Guid.NewGuid(), name, details, code, cost, price, quantity, alertQuantity, trackQuantity);
    }
    public static Product Create(Guid id, string name, string details, string code, decimal cost, decimal price, decimal quantity, decimal alertQuantity, bool trackQuantity)
    {
        Product product = new()
        {
            Id = id,
            Name = name,
            Details = details,
            Code = code,
            Slug = GetProductSlug(name),
            Cost = cost,
            AlertQuantity = alertQuantity,
            TrackQuantity = trackQuantity,
            Quantity = quantity,
            Active = true,
            Price = price
        };

        product.AddDomainEvent(new ProductCreatedEvent
        {
            Id = product.Id,
            Name = product.Name,
            Quantity = product.Quantity,
            Price = product.Price
        });

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
