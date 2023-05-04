using FluentPos.Catalog.Core.Products.Dtos;
using FluentValidation;
using FSH.Framework.Core.Events;
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
        public Validator(IProductRepository _repository)
        {
            RuleFor(p => p.AddProductDto.Name)
                .NotEmpty()
                .MaximumLength(75)
                .WithName("Name");

            RuleFor(p => p.AddProductDto.Cost)
                .GreaterThanOrEqualTo(1)
                .WithName("Cost");

            RuleFor(p => p.AddProductDto.Price)
                .GreaterThanOrEqualTo(1)
                .GreaterThanOrEqualTo(p => p.AddProductDto.Cost)
                .WithName("Price");

            RuleFor(p => p.AddProductDto.Code)
                .NotEmpty()
                .MaximumLength(75)
                .WithName("Code")
                .MustAsync(async (code, ct) => !await _repository.ExistsAsync(p => p.Code == code))
                .WithMessage((_, code) => $"Product with Code '{code}' already Exists.");
        }
    }
    public sealed class Handler : IRequestHandler<Command, ProductDto>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEventBus _eventBus;

        public Handler(IProductRepository repository, IMapper mapper, IEventBus eventBus)
        {
            _repository = repository;
            _mapper = mapper;
            _eventBus = eventBus;
        }

        public async Task<ProductDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var productToAdd = Product.Create(request.AddProductDto);
            await _repository.AddAsync(productToAdd);
            foreach (var @event in productToAdd.DomainEvents)
            {
                await _eventBus.PublishAsync(@event);
            }
            return _mapper.Map<ProductDto>(productToAdd);
        }
    }
}
