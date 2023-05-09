using Dapr.Client;
using FluentPos.Cart.Core.Dtos;
using FluentPos.Cart.Core.Exceptions;
using FluentPos.Shared.Events;
using FluentValidation;
using FSH.Framework.Core.Events;
using FSH.Framework.Infrastructure.Dapr;
using MediatR;

namespace FluentPos.Cart.Core.Features;
public static class CheckoutCart
{
    public sealed record Command : IRequest
    {

        public readonly Guid CustomerId;
        public readonly CheckoutCartRequestDto CheckoutRequest;

        public Command(CheckoutCartRequestDto checkoutRequest, Guid customerId)
        {
            CheckoutRequest = checkoutRequest;
            CustomerId = customerId;
        }
    }
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
        }
    }
    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly DaprClient _daprClient;
        private readonly IEventBus _eventBus;

        public Handler(DaprClient daprClient, IEventBus eventBus)
        {
            _daprClient = daprClient;
            _eventBus = eventBus;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var cart = await _daprClient.GetStateAsync<CustomerCart>(DaprConstants.RedisStateStore, request.CustomerId.ToString());
            if (cart == null)
            {
                throw new CartNotFoundException(request.CustomerId);
            }

            var cartCheckedOutEvent = new CartCheckedOutEvent(request.CustomerId, request.CheckoutRequest.CreditCardNumber!);
            await _eventBus.PublishIntegrationEventAsync(cartCheckedOutEvent);
        }
    }
}
