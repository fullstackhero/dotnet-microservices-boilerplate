using AutoMapper;
using Catalog.Domain.Entities;
using FSH.Core.Dto;

namespace Catalog.Application.Products;

public record ProductDto(Guid Id, string Name, int Quantity, decimal Price) : IDto;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductDto, Product>().ReverseMap();
    }
}
