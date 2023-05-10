using Dapr.Client;
using FluentValidation;
using FSH.Framework.Core.Events;
using FSH.Framework.Infrastructure.Dapr;
using MediatR;

namespace FluentPos.Cart.Core.Features;
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
        private readonly DaprClient _daprClient;
        private readonly IEventBus _eventBus;

        public Handler(DaprClient daprClient, IEventBus eventBus)
        {
            _daprClient = daprClient;
            _eventBus = eventBus;
        }

        public async Task<CustomerCart> Handle(Query request, CancellationToken cancellationToken)
        {
            var cart = await _daprClient.GetStateAsync<CustomerCart>(DaprConstants.RedisStateStore, request.CustomerId.ToString(), cancellationToken: cancellationToken);
            return cart!;
        }
    }
}