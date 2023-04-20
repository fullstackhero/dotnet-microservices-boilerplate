using FluentPos.Catalog.Core.Products.Dtos;
using FluentValidation;
using FSH.Microservices.Core.Database;
using MapsterMapper;
using MediatR;

namespace FluentPos.Catalog.Core.Products.Features;
public static class AddProduct
{
    public sealed record Command : IRequest<ProductDto>
    {
        public readonly AddProductDto AddProductDto;
        public Command(AddProductDto addProductDto)
        {
            AddProductDto = addProductDto;
        }
    }
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(p => p.AddProductDto.Name).NotEmpty().MaximumLength(75).WithName("Name");
            RuleFor(p => p.AddProductDto.Cost).GreaterThanOrEqualTo(1).WithName("Cost");
            RuleFor(p => p.AddProductDto.Price).GreaterThanOrEqualTo(1).GreaterThanOrEqualTo(p => p.AddProductDto.Cost).WithName("Price");
            RuleFor(p => p.AddProductDto.Code).NotEmpty().MaximumLength(75).WithName("Code");
        }
    }
    public sealed class Handler : IRequestHandler<Command, ProductDto>
    {
        private readonly IRepository<Product, Guid> _repository;
        private readonly IMapper _mapper;

        public Handler(IRepository<Product, Guid> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var productToAdd = Product.Create(request.AddProductDto);
            await _repository.AddAsync(productToAdd);
            return _mapper.Map<ProductDto>(productToAdd);
        }
    }
}
