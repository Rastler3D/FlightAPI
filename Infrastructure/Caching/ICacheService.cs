namespace Infrastructure.Cache
{
}

namespace Infrastructure.Cache
{
    internal interface ICacheService
    {
        Task<T?> GetAsync<T>(string key)
            where T : class;

        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> valueFactory) where T : class;
        Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null) where T : class;
        Task<bool> RemoveAsync(string key);
        Task<long> RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default);
    }
}