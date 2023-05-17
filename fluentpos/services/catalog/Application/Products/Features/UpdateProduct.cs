using FluentPos.Catalog.Application.Products.Dtos;
using FluentPos.Catalog.Application.Products.Exceptions;
using FluentPos.Catalog.Domain.Products;
using FSH.Framework.Core.Caching;
using MapsterMapper;
using MediatR;

namespace FluentPos.Catalog.Application.Products.Features;
public static class UpdateProduct
{
    public sealed record Command : IRequest<ProductDto>
    {
        public readonly UpdateProductDto UpdateProductDto;
        public readonly Guid Id;
        public Command(UpdateProductDto updateProductDto, Guid id)
        {
            UpdateProductDto = updateProductDto;
            Id = id;
        }
    }
    public sealed class Handler : IRequestHandler<Command, ProductDto>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public Handler(IProductRepository repository, IMapper mapper, ICacheService cacheService)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<ProductDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var productToBeUpdated = await _repository.FindByIdAsync(request.Id, cancellationToken) ?? throw new ProductNotFoundException(request.Id);
            productToBeUpdated.Update(
                request.UpdateProductDto.Name,
                request.UpdateProductDto.Details,
                request.UpdateProductDto.Price,
                request.UpdateProductDto.Cost,
                request.UpdateProductDto.TrackQuantity,
                request.UpdateProductDto.AlertQuantity,
                request.UpdateProductDto.Quantity);

            await _repository.UpdateAsync(productToBeUpdated, cancellationToken);
            await _cacheService.RemoveAsync(Product.GetCacheKey(request.Id), cancellationToken);
            return _mapper.Map<ProductDto>(productToBeUpdated);
        }
    }
}
