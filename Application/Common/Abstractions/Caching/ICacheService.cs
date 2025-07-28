namespace Application.Common.Abstractions.Caching;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default);
    Task<T> GetOrSetAsync<T>(string key, Func<CancellationToken, Task<T>> valueFactory, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
}