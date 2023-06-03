using FluentPos.Cart.Application.Dtos;
using FluentPos.Cart.Application.Exceptions;
using FluentPos.Shared.Events;
using FluentValidation;
using FSH.Framework.Core.Events;
using MediatR;

namespace FluentPos.Cart.Application.Features;
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
        private readonly ICartRepository _cartRepository;
        private readonly IEventPublisher _eventBus;

        public Handler(IEventPublisher eventBus, ICartRepository cartRepository)
        {
            _eventBus = eventBus;
            _cartRepository = cartRepository;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            _ = await _cartRepository.GetCustomerCartAsync(request.CustomerId.ToString(), cancellationToken) ?? throw new CartNotFoundException(request.CustomerId);
            var cartCheckedOutEvent = new CartCheckedOutEvent(request.CustomerId, request.CheckoutRequest.CreditCardNumber!);
            await _eventBus.PublishAsync(cartCheckedOutEvent, token: cancellationToken);
        }
    }
}
