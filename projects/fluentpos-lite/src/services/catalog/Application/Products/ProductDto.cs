using AutoMapper;
using FluentPOS.Lite.Catalog.Domain.Entities;
using FSH.Core.Dto;

namespace FluentPOS.Lite.Catalog.Application.Products;

public class ProductDto : IDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public string? Code { get; set; }
    public decimal Price { get; set; }
    public decimal AvailableQuantity { get; set; }
}
public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>().ForMember(dest => dest.AvailableQuantity, opt => opt.MapFrom(src => src.Quantity));
        CreateMap<ProductDto, Product>();
    }
}
