using FSH.Framework.Core.Domain;
using System.Text.RegularExpressions;

namespace FluentPos.Catalog.Domain.Products;

public class Product : BaseEntity
{
    public string Name { get; private set; } = default!;
    public string Details { get; private set; } = default!;
    public string Code { get; private set; } = default!;
    public string Slug { get; private set; } = default!;
    public bool Active { get; private set; } = true;
    public decimal? Cost { get; private set; }
    public decimal? Price { get; private set; }
    public decimal? Quantity { get; private set; }
    public decimal? AlertQuantity { get; private set; }
    public bool? TrackQuantity { get; private set; }

    public Product Update(
        string? name,
        string? details,
        decimal? price,
        decimal? cost,
        bool? trackQuantity,
        decimal? alertQuantity,
        decimal? quantity)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (details is not null && Details?.Equals(details) is not true) Details = details;
        if (price.HasValue && Price != price.Value) Price = price.Value;
        if (cost.HasValue && Cost != cost.Value) Cost = cost.Value;
        if (trackQuantity.HasValue && TrackQuantity != trackQuantity) TrackQuantity = trackQuantity.Value;
        if (alertQuantity.HasValue && AlertQuantity != alertQuantity.Value) AlertQuantity = alertQuantity.Value;
        if (quantity.HasValue && Quantity != quantity.Value) Quantity = quantity.Value;
        return this;
    }
    public static Product Create(
        string? name,
        string? details,
        string? code,
        decimal? cost,
        decimal? price,
        decimal? alertQuantity,
        bool? trackQuantity,
        decimal? quantity)
    {
        Product product = new()
        {
            Name = name!,
            Details = details!,
            Code = code!,
            Slug = GetProductSlug(name!),
            Cost = cost,
            AlertQuantity = alertQuantity,
            TrackQuantity = trackQuantity,
            Quantity = quantity,
            Active = true,
            Price = price
        };

        var @event = new ProductCreatedEvent(product.Id, product.Name);
        product.AddDomainEvent(@event);

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
