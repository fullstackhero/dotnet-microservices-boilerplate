using FluentPos.Catalog.Application.Products.Dtos;
using FluentPos.Catalog.Domain.Products;
using Mapster;

namespace FluentPos.Catalog.Application.Products.Mappings;
public sealed class ProductMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductDto>();
        config.NewConfig<Product, ProductDetailsDto>();
    }
}