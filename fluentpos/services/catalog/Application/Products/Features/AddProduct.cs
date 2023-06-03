using FluentPos.Catalog.Application.Products.Dtos;
using FluentPos.Catalog.Domain.Products;
using FluentValidation;
using FSH.Framework.Core.Events;
using MapsterMapper;
using MediatR;

namespace FluentPos.Catalog.Application.Products.Features;
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
                .MustAsync(async (code, ct) => !await _repository.ExistsAsync(p => p.Code == code, ct))
                .WithMessage((_, code) => $"Product with Code '{code}' already Exists.");
        }
    }
    public sealed class Handler : IRequestHandler<Command, ProductDto>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventBus;

        public Handler(IProductRepository repository, IMapper mapper, IEventPublisher eventBus)
        {
            _repository = repository;
            _mapper = mapper;
            _eventBus = eventBus;
        }

        public async Task<ProductDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var productToAdd = Product.Create(
                request.AddProductDto.Name,
                request.AddProductDto.Details,
                request.AddProductDto.Code,
                request.AddProductDto.Cost,
                request.AddProductDto.Price,
                request.AddProductDto.AlertQuantity,
                request.AddProductDto.TrackQuantity,
                request.AddProductDto.Quantity);

            await _repository.AddAsync(productToAdd, cancellationToken);
            foreach (var @event in productToAdd.DomainEvents)
            {
                await _eventBus.PublishAsync(@event, token: cancellationToken);
            }
            return _mapper.Map<ProductDto>(productToAdd);
        }
    }
}
