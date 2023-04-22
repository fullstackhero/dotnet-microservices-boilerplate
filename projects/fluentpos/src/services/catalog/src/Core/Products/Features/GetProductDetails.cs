using FluentPos.Catalog.Core.Products.Dtos;
using FluentPos.Catalog.Core.Products.Exceptions;
using MapsterMapper;
using MediatR;

namespace FluentPos.Catalog.Core.Products.Features;
public static class GetProductDetails
{
    public sealed record Query : IRequest<ProductDetailsDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Query, ProductDetailsDto>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public Handler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductDetailsDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var product = await _repository.FindByIdAsync(request.Id, cancellationToken);
            if (product == null) throw new ProductNotFoundException(request.Id);
            return _mapper.Map<ProductDetailsDto>(product);
        }
    }
}

