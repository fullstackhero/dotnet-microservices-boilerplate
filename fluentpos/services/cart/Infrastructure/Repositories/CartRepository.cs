using FluentPos.Cart.Application;
using FluentPos.Cart.Domain;
using FSH.Framework.Core.Caching;

namespace FluentPos.Cart.Infrastructure.Repositories;
public class CartRepository : ICartRepository
{
    private readonly ICacheService _cacheService;

    public CartRepository(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<CustomerCart?> GetCustomerCartAsync(string customerId, CancellationToken cancellationToken)
    {
        string cacheKey = $"cart-{customerId}";
        return await _cacheService.GetAsync<CustomerCart>(cacheKey, token: cancellationToken);
    }

    public async Task UpdateCustomerCartAsync(string customerId, CustomerCart cart, CancellationToken cancellationToken)
    {
        string cacheKey = $"cart-{customerId}";
        await _cacheService.SetAsync<CustomerCart>(cacheKey, cart, cancellationToken: cancellationToken);
    }
}
