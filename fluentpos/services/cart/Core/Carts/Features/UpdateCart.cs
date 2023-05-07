using Dapr.Client;
using FluentPos.Cart.Core.Carts.Dtos;
using FluentValidation;
using FSH.Framework.Core.Events;
using FSH.Framework.Infrastructure.Dapr;
using MediatR;

namespace FluentPos.Cart.Core.Carts.Features;
public static class UpdateCart
{
    public sealed record Command : IRequest<CustomerCart>
    {

        public readonly Guid CustomerId;
        public readonly UpdateCartDto UpdateCartDto;

        public Command(UpdateCartDto updateCartDto, Guid customerId)
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
        private readonly DaprClient _daprClient;
        private readonly IEventBus _eventBus;

        public Handler(DaprClient daprClient, IEventBus eventBus)
        {
            _daprClient = daprClient;
            _eventBus = eventBus;
        }

        public async Task<CustomerCart> Handle(Command request, CancellationToken cancellationToken)
        {
            var customerId = request.CustomerId.ToString();
            var cart = await _daprClient.GetStateAsync<CustomerCart>(DaprConstants.RedisStateStore, customerId);
            if (cart == null) cart = new CustomerCart(request.CustomerId);
            foreach (var item in request.UpdateCartDto.Items)
            {
                cart.AddItem(item.ProductId, item.Quantity);
            }
            await _daprClient.SaveStateAsync(DaprConstants.RedisStateStore, customerId, cart);
            return cart!;
        }
    }
}
