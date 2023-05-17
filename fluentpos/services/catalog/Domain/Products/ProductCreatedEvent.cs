using FSH.Framework.Core.Events;

namespace FluentPos.Catalog.Domain.Products;
public class ProductCreatedEvent : DomainEvent
{
    public Guid ProductId { get; }
    public string ProductName { get; }

    public ProductCreatedEvent(Guid productId, string productName)
    {
        ProductId = productId;
        ProductName = productName;
    }
}