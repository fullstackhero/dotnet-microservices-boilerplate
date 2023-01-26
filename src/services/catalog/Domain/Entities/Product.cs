using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FSH.Core.Domain;
using FSH.Shared.Events.Catalog;

namespace Catalog.Domain.Entities;

public class Product : EntityRootBase
{
    public string Name { get; private init; } = default!;
    public bool Active { get; private init; }
    public int Quantity { get; private init; }
    public decimal Price { get; private init; }

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
            Created = DateTime.UtcNow,
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
}
