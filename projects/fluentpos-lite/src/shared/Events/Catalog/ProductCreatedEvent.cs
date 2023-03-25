using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FSH.Core.Domain;

namespace FluentPOS.Lite.Events.Catalog;

public class ProductCreatedEvent : EventBase
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Quantity { get; set; }
    public Guid ProductCodeId { get; set; }
    public decimal Price { get; set; }

    public override void Flatten()
    {
        MetaData.Add("ProductId", Id);
        MetaData.Add("ProductName", Name);
        MetaData.Add("ProductQuantity", Quantity);
        MetaData.Add("ProductCode", ProductCodeId);
        MetaData.Add("ProductCost", Price);
    }
}
