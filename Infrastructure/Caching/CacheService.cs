using System.Text.Json;
using Application.Common.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Infrastructure.Caching;

internal sealed class CacheService(
    IDistributedCache cache,
    IConnectionMultiplexer redis,
    ILogger<CacheService> logger)
    : ICacheService
{
    
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(10);

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            var cachedValue = await cache.GetStringAsync(key, cancellationToken);
            
            if (string.IsNullOrEmpty(cachedValue))
                return default;

            return JsonSerializer.Deserialize<T>(cachedValue);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving from cache with key {Key}", key);
            return default;
        }
    }
    
    public async Task<T> GetOrSetAsync<T>(string key, Func<CancellationToken, Task<T>> valueFactory, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        var cachedValue = await GetAsync<T>(key, cancellationToken: cancellationToken);

        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var newCachedValue = await valueFactory(cancellationToken);

        await SetAsync(key, newCachedValue, cancellationToken: cancellationToken);

        return newCachedValue;
    }
    
    
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var serializedValue = JsonSerializer.Serialize(value);
            
            var options = new DistributedCacheEntryOptions();
            
            options.SetAbsoluteExpiration(expiration ?? DefaultExpiration);

            await cache.SetStringAsync(key, serializedValue, options, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error setting cache with key {Key}", key);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await cache.RemoveAsync(key, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error removing from cache with key {Key}", key);
        }
    }
    

    public async Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
    {
        try
        {
            var database = redis.GetDatabase();
            var server = redis.GetServer(redis.GetEndPoints().First());
            
            var keyStream = server.KeysAsync(pattern: pattern);
            
            var keys = new List<RedisKey>();
            
            await foreach (var key in keyStream.WithCancellation(cancellationToken))
            {
                keys.Add(key);
            }
            if (keys.Count > 0)
            {
                await database.KeyDeleteAsync(keys.ToArray());
                logger.LogInformation("Removed {Count} keys matching pattern {Pattern}", keys.Count, pattern);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error removing from cache with pattern {Pattern}", pattern);
        }
    }
}