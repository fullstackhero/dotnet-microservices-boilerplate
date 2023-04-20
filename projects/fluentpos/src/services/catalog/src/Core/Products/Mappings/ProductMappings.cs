using FluentPos.Catalog.Core.Products.Dtos;
using Mapster;

namespace FluentPos.Catalog.Core.Products.Mappings;
public sealed class ProductMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductDto>();
    }
}