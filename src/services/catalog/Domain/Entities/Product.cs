using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FSH.Core.Domain;
using FSH.Shared.Events.Catalog;

namespace Catalog.Domain.Entities;

public class Product : AuditableEntity
{
    public string Name { get; private set; } = default!;
    public bool Active { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    public Product Update(string? name, int quantity, decimal? price)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        Quantity = quantity;
        if (price.HasValue && Price != price) Price = price.Value;
        return this;
    }

    public static Product Create(string name, int quantity, decimal price)
    {
        return Create(Guid.NewGuid(), name, quantity, price);
    }
    public static Product Create(Guid id, string name, int quantity, decimal price)
    {
        Product product = new()
        {
            Id = id,
            Name = name,
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

    public static string GenerateCacheKey(Guid id)
    {
        return $"Product:{id}";
    }
}
