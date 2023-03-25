using AutoMapper;
using FluentPOS.Lite.Catalog.Domain.Entities;
using FSH.Core.Dto;

namespace FluentPOS.Lite.Catalog.Application.Products;

public record ProductDto(Guid Id, string Name, string Slug, decimal Price, decimal AvailableQuantity) : IDto;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductDto, Product>().ReverseMap()
        .ForMember(dest => dest.AvailableQuantity, opt => opt.MapFrom(src => src.Quantity));
    }
}
