using System.Text;
using Dapr.Client;
using FSH.Core.Common;
using Microsoft.Extensions.Logging;

namespace FSH.Infrastructure.Caching;

public class DaprCacheService : ICacheService
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<DaprCacheService> _logger;
    public DaprCacheService(DaprClient daprClient, ILogger<DaprCacheService> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public T Get<T>(string key)
    {
        throw new NotImplementedException();
    }

    public async Task<T> GetAsync<T>(string key, CancellationToken token = default)
    {
        try
        {
            var data = await _daprClient.GetStateAsync<T>("statestore", key, ConsistencyMode.Eventual, cancellationToken: token);
            _logger.LogInformation("Getting Cache for Key : {key}", key);
            return data;
        }
        catch (Exception ex)
        {
            _logger.LogError("Unable to Get Cache with Key {key} : {Message}", key, ex.Message);
            return default;
        }
    }

    public void Refresh(string key)
    {
        throw new NotImplementedException();
    }

    public Task RefreshAsync(string key, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public void Remove(string key)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveAsync(string key, CancellationToken token = default)
    {
        try
        {
            await _daprClient.DeleteStateAsync("statestore", key, cancellationToken: token);
            _logger.LogInformation("Removed Cache for Key : {key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError("Unable to Remove Cache with Key {key} : {Message}", key, ex.Message);
        }
    }

    public void Set<T>(string key, T value, TimeSpan? slidingExpiration = null)
    {
        throw new NotImplementedException();
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
    {
        try
        {
            await _daprClient.SaveStateAsync("statestore", key, value, cancellationToken: cancellationToken);
            _logger.LogInformation("Set Cache for Key : {key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError("Unable to Set Cache with Key {key} : {Message}", key, ex.Message);
        }
    }
}
