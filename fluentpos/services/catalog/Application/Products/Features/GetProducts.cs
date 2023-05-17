using FluentPos.Catalog.Application.Products.Dtos;
using FSH.Framework.Core.Pagination;
using MediatR;

namespace FluentPos.Catalog.Application.Products.Features;
public static class GetProducts
{
    public sealed record Query : IRequest<PagedList<ProductDto>>
    {
        public readonly ProductsParametersDto Parameters;

        public Query(ProductsParametersDto parameters)
        {
            Parameters = parameters;
        }
    }

    public sealed class Handler : IRequestHandler<Query, PagedList<ProductDto>>
    {
        private readonly IProductRepository _repository;

        public Handler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedList<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _repository.GetPagedProductsAsync<ProductDto>(request.Parameters, cancellationToken);
        }
    }
}
