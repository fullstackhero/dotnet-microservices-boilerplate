namespace FluentPos.Catalog.Application.Products.Dtos;
public sealed class AddProductDto
{
    public string? Name { get; set; }
    public string? Details { get; set; }
    public string? Code { get; set; }
    public decimal Cost { get; set; }
    public decimal Price { get; set; }
    public decimal Quantity { get; set; } = 0;
    public decimal AlertQuantity { get; set; } = 10;
    public bool TrackQuantity { get; set; } = true;
}