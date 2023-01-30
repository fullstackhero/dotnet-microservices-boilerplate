using System.Text;
using Dapr.Client;
using FSH.Core.Common;
using Microsoft.Extensions.Logging;

namespace FSH.Infrastructure.Caching;

public class DaprCacheService : ICacheService
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<DaprCacheService> _logger;

    private readonly ISerializationService _serializer;

    public DaprCacheService(DaprClient daprClient, ILogger<DaprCacheService> logger, ISerializationService serializer)
    {
        _daprClient = daprClient;
        _logger = logger;
        _serializer = serializer;
    }

    public T Get<T>(string key)
    {
        throw new NotImplementedException();
    }

    public async Task<T> GetAsync<T>(string key, CancellationToken token = default) =>
        await _daprClient.GetStateAsync<T>("statestore", key, ConsistencyMode.Eventual, cancellationToken: token);

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
        }
        catch
        {
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
        }
        catch
        {
        }
    }
}
