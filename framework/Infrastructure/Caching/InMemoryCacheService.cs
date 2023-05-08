using FSH.Framework.Core.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FSH.Framework.Infrastructure.Caching;

public class InMemoryCacheService : ICacheService
{
    private readonly ILogger<InMemoryCacheService> _logger;
    private readonly IMemoryCache _cache;
    private readonly CachingOptions _cacheOptions;
    public InMemoryCacheService(IMemoryCache cache, ILogger<InMemoryCacheService> logger, IOptions<CachingOptions> cacheOptions)
    {
        _cache = cache;
        _logger = logger;
        _cacheOptions = cacheOptions.Value;
    }

    public T Get<T>(string key) => _cache.Get<T>(key)!;

    public Task<T> GetAsync<T>(string key, CancellationToken token = default)
    {
        var data = Get<T>(key)!;
        if (data != null)
        {
            _logger.LogDebug("Get From Cache : {key}", key);
        }
        else
        {
            _logger.LogDebug("Key Not Found in Cache : {key}", key);
        }
        return Task.FromResult(data);
    }

    public void Refresh(string key) => _cache.TryGetValue(key, out _);

    public Task RefreshAsync(string key, CancellationToken token = default)
    {
        Refresh(key);
        return Task.CompletedTask;
    }

    public void Remove(string key) => _cache.Remove(key);

    public Task RemoveAsync(string key, CancellationToken token = default)
    {
        Remove(key);
        return Task.CompletedTask;
    }

    public void Set<T>(string key, T value, TimeSpan? slidingExpiration = null, DateTimeOffset? absoluteExpiration = null)
    {
        slidingExpiration ??= TimeSpan.FromMinutes(_cacheOptions.SlidingExpirationInMinutes);
        absoluteExpiration ??= DateTime.UtcNow.AddMinutes(_cacheOptions.AbsoluteExpirationInMinutes);
        _cache.Set(key, value, new MemoryCacheEntryOptions { SlidingExpiration = slidingExpiration, AbsoluteExpiration = absoluteExpiration });
        _logger.LogDebug("Added to Cache : {key}", key);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, DateTimeOffset? absoluteExpiration = null, CancellationToken token = default)
    {
        Set(key, value, slidingExpiration);
        return Task.CompletedTask;
    }
}