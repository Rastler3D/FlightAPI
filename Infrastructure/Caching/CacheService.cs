using System.Text.Json;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Infrastructure.Caching;

internal sealed class CacheService(IConnectionMultiplexer connection, ILogger<CacheService> logger) : ICacheService
{
    private readonly IDatabase _database = connection.GetDatabase();

    public async Task<T?> GetAsync<T>(string key)
        where T : class
    {
        try
        {
            var resultString = await _database.StringGetAsync(key);
            var result = !string.IsNullOrEmpty(resultString)
                ? JsonSerializer.Deserialize<T>(resultString!)
                : null;

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получение значения {CacheKey} из кэша", key);

            return null;
        }
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> valueFactory) where T : class
    {
        var cachedValue = await GetAsync<T>(key);

        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var newCachedValue = await valueFactory();

        await SetAsync(key, newCachedValue);

        return newCachedValue;
    }

    public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null) where T : class
    {
        try
        {
            var redisValue = JsonSerializer.Serialize(value);

            return await _database.StringSetAsync(key, redisValue, expiry);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Не удалось сохранить значения {CacheKey} в кэш", key);

            return false;
        }
    }

    public async Task<bool> RemoveAsync(string key)
    {
        try
        {
            return await _database.KeyDeleteAsync(key);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Не удалось удалить значение {CacheKey} из кэша", key);
            return false;
        }
    }

    public async Task<long> RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        var endPoint = connection.GetEndPoints().First();
        var server = connection.GetServer(endPoint);
        var pattern = $"{prefix}*";
        var keys = new List<RedisKey>();
        ;

        await foreach (var key in server.KeysAsync(pattern: pattern).WithCancellation(cancellationToken))
        {
            keys.Add(key);
        }

        return await _database.KeyDeleteAsync(keys.ToArray());
    }
}