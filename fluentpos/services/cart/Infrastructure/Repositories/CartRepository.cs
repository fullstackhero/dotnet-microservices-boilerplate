using Dapr.Client;
using FluentPos.Cart.Application;
using FluentPos.Cart.Domain;
using FSH.Framework.Infrastructure.Dapr;

namespace FluentPos.Cart.Infrastructure.Repositories;
public class CartRepository : ICartRepository
{
    private readonly DaprClient _daprClient;

    public CartRepository(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task<CustomerCart?> GetCustomerCartAsync(string customerId, CancellationToken cancellationToken)
    {
        return await _daprClient.GetStateAsync<CustomerCart>(DaprConstants.RedisStateStore, customerId, cancellationToken: cancellationToken);
    }

    public async Task UpdateCustomerCartAsync(string customerId, CustomerCart cart, CancellationToken cancellationToken)
    {
        await _daprClient.SaveStateAsync(DaprConstants.RedisStateStore, customerId, cart, cancellationToken: cancellationToken);
    }
}
