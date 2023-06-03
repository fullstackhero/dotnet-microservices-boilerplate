using FluentPos.Cart.Application.Exceptions;
using FluentPos.Cart.Domain;
using FluentValidation;
using MediatR;

namespace FluentPos.Cart.Application.Features;
public static class GetCart
{
    public sealed record Query : IRequest<CustomerCart>
    {
        public readonly Guid CustomerId;

        public Query(Guid customerId)
        {
            CustomerId = customerId;
        }
    }
    public sealed class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(p => p.CustomerId).NotEmpty();
        }
    }
    public sealed class Handler : IRequestHandler<Query, CustomerCart>
    {
        private readonly ICartRepository _cartRepository;

        public Handler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<CustomerCart> Handle(Query request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetCustomerCartAsync(request.CustomerId.ToString(), cancellationToken);
            if (cart == null) throw new CartNotFoundException(request.CustomerId.ToString());
            return cart;
        }
    }
}