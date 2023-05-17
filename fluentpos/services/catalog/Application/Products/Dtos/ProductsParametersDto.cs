using FSH.Framework.Core.Pagination;

namespace FluentPos.Catalog.Application.Products.Dtos;
public class ProductsParametersDto : PaginationParameters
{
    public string? Keyword { get; set; }
}
