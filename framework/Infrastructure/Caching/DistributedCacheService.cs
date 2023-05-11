using System.Text;
using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Serializers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace FSH.Framework.Infrastructure.Caching
{
    internal class DistributedCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<DistributedCacheService> _logger;
        private readonly ISerializerService _serializer;

        public DistributedCacheService(IDistributedCache cache, ISerializerService serializer, ILogger<DistributedCacheService> logger) =>
            (_cache, _serializer, _logger) = (cache, serializer, logger);

        public T Get<T>(string key) =>
            Get(key) is { } data
                ? Deserialize<T>(data)
                : default!;

        private byte[] Get(string key)
        {
            ArgumentNullException.ThrowIfNull(key);

            try
            {
                return _cache.Get(key)!;
            }
            catch
            {
                return default!;
            }
        }

        public async Task<T> GetAsync<T>(string key, CancellationToken token = default) =>
            await GetAsync(key, token) is { } data
                ? Deserialize<T>(data)
                : default!;

        private async Task<byte[]> GetAsync(string key, CancellationToken token = default)
        {
            try
            {
                byte[]? data = await _cache.GetAsync(key, token)!;
                return data!;
            }
            catch
            {
                return default!;
            }
        }

        public void Refresh(string key)
        {
            try
            {
                _cache.Refresh(key);
            }
            catch
            {
            }
        }

        public async Task RefreshAsync(string key, CancellationToken token = default)
        {
            try
            {
                await _cache.RefreshAsync(key, token);
                _logger.LogDebug("Cache Refreshed : {key}", key);
            }
            catch
            {
            }
        }

        public void Remove(string key)
        {
            try
            {
                _cache.Remove(key);
            }
            catch
            {
            }
        }

        public async Task RemoveAsync(string key, CancellationToken token = default)
        {
            try
            {
                await _cache.RemoveAsync(key, token);
            }
            catch
            {
            }
        }

        public void Set<T>(string key, T value, TimeSpan? slidingExpiration = null, DateTimeOffset? absoluteExpiration = null) =>
            Set(key, Serialize(value), slidingExpiration);

        private void Set(string key, byte[] value, TimeSpan? slidingExpiration = null, DateTimeOffset? absoluteExpiration = null)
        {
            try
            {
                _cache.Set(key, value, GetOptions(slidingExpiration, absoluteExpiration));
                _logger.LogDebug("Added to Cache : {key}", key);
            }
            catch
            {
            }
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, DateTimeOffset? absoluteExpiration = null, CancellationToken cancellationToken = default) =>
            SetAsync(key, Serialize(value), slidingExpiration, absoluteExpiration, cancellationToken);

        private async Task SetAsync(string key, byte[] value, TimeSpan? slidingExpiration = null, DateTimeOffset? absoluteExpiration = null, CancellationToken token = default)
        {
            try
            {
                await _cache.SetAsync(key, value, GetOptions(slidingExpiration, absoluteExpiration), token);
                _logger.LogDebug("Added to Cache : {key}", key);
            }
            catch
            {
            }
        }

        private byte[] Serialize<T>(T item)
        {
            return Encoding.Default.GetBytes(_serializer.Serialize(item));
        }

        private T Deserialize<T>(byte[] cachedData) =>
            _serializer.Deserialize<T>(Encoding.Default.GetString(cachedData));

        private static DistributedCacheEntryOptions GetOptions(TimeSpan? slidingExpiration, DateTimeOffset? absoluteExpiration)
        {
            var options = new DistributedCacheEntryOptions();
            if (slidingExpiration.HasValue)
            {
                options.SetSlidingExpiration(slidingExpiration.Value);
            }
            else
            {
                options.SetSlidingExpiration(TimeSpan.FromMinutes(10)); // Default expiration time of 10 minutes.
            }

            if (absoluteExpiration.HasValue)
            {
                options.SetAbsoluteExpiration(absoluteExpiration.Value);
            }
            else
            {
                options.SetAbsoluteExpiration(TimeSpan.FromMinutes(15)); // Default expiration time of 10 minutes.
            }

            return options;
        }
    }
}
