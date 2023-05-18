using FluentPos.Cart.Application.Dtos;
using FluentPos.Cart.Domain;
using FluentValidation;
using MediatR;

namespace FluentPos.Cart.Application.Features;
public static class UpdateCart
{
    public sealed record Command : IRequest<CustomerCart>
    {
        public readonly Guid CustomerId;
        public readonly UpdateCartRequestDto UpdateCartDto;

        public Command(UpdateCartRequestDto updateCartDto, Guid customerId)
        {
            UpdateCartDto = updateCartDto;
            CustomerId = customerId;
        }
    }
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(dto => dto.UpdateCartDto.Items)
            .NotEmpty().WithMessage("At least one item must be specified.")
            .Must(items => items.All(item => item.Quantity > 0))
                .WithMessage("Quantity of each product must be greater than 0.");
        }
    }
    public sealed class Handler : IRequestHandler<Command, CustomerCart>
    {
        private readonly ICartRepository _cartRepository;

        public Handler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<CustomerCart> Handle(Command request, CancellationToken cancellationToken)
        {
            string customerId = request.CustomerId.ToString();
            var cart = await _cartRepository.GetCustomerCartAsync(customerId, cancellationToken) ?? new CustomerCart(request.CustomerId);
            foreach (var item in request.UpdateCartDto.Items)
            {
                cart.AddItem(item.ProductId, item.Quantity);
            }
            await _cartRepository.UpdateCustomerCartAsync(customerId, cart, cancellationToken);
            return cart!;
        }
    }
}
