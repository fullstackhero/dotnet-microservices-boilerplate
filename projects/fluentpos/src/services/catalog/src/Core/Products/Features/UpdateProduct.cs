using FluentPos.Catalog.Core.Products.Dtos;
using FluentPos.Catalog.Core.Products.Exceptions;
using MapsterMapper;
using MediatR;

namespace FluentPos.Catalog.Core.Products.Features;
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

        public Handler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var productToBeUpdated = await _repository.FindByIdAsync(request.Id, cancellationToken);
            if (productToBeUpdated == null) throw new ProductNotFoundException(request.Id);
            productToBeUpdated.Update(request.UpdateProductDto);
            await _repository.UpdateAsync(productToBeUpdated, cancellationToken);
            return _mapper.Map<ProductDto>(productToBeUpdated);
        }
    }
}

