using FluentPos.Catalog.Core.Products.Dtos;
using FSH.Framework.Core.Pagination;
using MapsterMapper;
using MediatR;

namespace FluentPos.Catalog.Core.Products.Features;
public class GetProducts
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
        private readonly IMapper _mapper;

        public Handler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _repository.GetPagedProductsAsync<ProductDto>(request.Parameters, cancellationToken);
        }
    }
}
