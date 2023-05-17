using FluentPos.Catalog.Application.Products.Dtos;
using FluentPos.Catalog.Application.Products.Exceptions;
using FluentPos.Catalog.Domain.Products;
using FSH.Framework.Core.Caching;
using MapsterMapper;
using MediatR;

namespace FluentPos.Catalog.Application.Products.Features;
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
        private readonly ICacheService _cache;
        private readonly IMapper _mapper;

        public Handler(IProductRepository repository, IMapper mapper, ICacheService cache)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<ProductDetailsDto> Handle(Query request, CancellationToken cancellationToken)
        {
            string cacheKey = Product.GetCacheKey(request.Id);
            var productDto = await _cache.GetAsync<ProductDetailsDto>(cacheKey, cancellationToken);
            if (productDto == null)
            {
                var product = await _repository.FindByIdAsync(request.Id, cancellationToken) ?? throw new ProductNotFoundException(request.Id);
                productDto = _mapper.Map<ProductDetailsDto>(product);
                await _cache.SetAsync(cacheKey, productDto, cancellationToken: cancellationToken);
            }
            return productDto;
        }
    }
}
