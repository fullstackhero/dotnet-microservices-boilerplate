namespace FluentPos.Catalog.Application.Products.Dtos;
public sealed class UpdateProductDto
{
    public string? Name { get; init; }
    public string? Details { get; init; }
    public decimal? Cost { get; init; } = null;
    public decimal? Price { get; init; } = null;
    public decimal? Quantity { get; init; } = null;
    public decimal? AlertQuantity { get; init; } = null;
    public bool? TrackQuantity { get; init; } = null;
}