namespace DDD.Application.Abstractions.Redis;

public interface IRedisCache
{
    /// <summary>
    /// Removes the cache entry with the specified key.
    /// </summary>
    /// <param name="key">The key of the cache entry to remove.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RemoveCacheAsync(string key);

    /// <summary>
    /// Removes cache entries that match the specified pattern.
    /// </summary>
    /// <param name="pattern">The pattern to match cache keys.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether any cache entries were removed.</returns>
    Task<bool> RemoveCacheByPatternAsync(string pattern);

    /// <summary>
    /// Gets the cache entry for the specified key. If the cache entry does not exist, the specified data function is executed to retrieve the data and set the cache.
    /// </summary>
    /// <typeparam name="T">The type of the cache entry.</typeparam>
    /// <param name="key">The key of the cache entry.</param>
    /// <param name="data">The function to retrieve the data if the cache entry does not exist.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <param name="absoluteExpiration">The absolute expiration date for the cache entry.</param>
    /// <param name="absoluteExpirationRelativeToNow">The relative expiration time from now for the cache entry.</param>
    /// <param name="slidingExpiration">The sliding expiration time for the cache entry.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the cache entry.</returns>
    Task<T> GetCacheAsync<T>(string key,
        Func<Task<T>> data,
        CancellationToken cancellationToken,
        DateTimeOffset? absoluteExpiration = null,
        TimeSpan? absoluteExpirationRelativeToNow = null,
        TimeSpan? slidingExpiration = null) where T : class;

    /// <summary>
    /// Gets the cache entry for the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the cache entry.</typeparam>
    /// <param name="key">The key of the cache entry.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the cache entry, or null if the cache entry does not exist.</returns>
    Task<T?> GetCacheAsync<T>(string key, CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Sets the cache entry for the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the cache entry.</typeparam>
    /// <param name="key">The key of the cache entry.</param>
    /// <param name="value">The value to set in the cache.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <param name="absoluteExpiration">The absolute expiration date for the cache entry.</param>
    /// <param name="absoluteExpirationRelativeToNow">The relative expiration time from now for the cache entry.</param>
    /// <param name="slidingExpiration">The sliding expiration time for the cache entry.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SetCacheAsync<T>(string key,
        T value,
        CancellationToken cancellationToken,
        DateTimeOffset? absoluteExpiration = null,
        TimeSpan? absoluteExpirationRelativeToNow = null,
        TimeSpan? slidingExpiration = null) where T : class;

    /// <summary>
    /// Gets all keys from.
    /// </summary>
    /// <param name="name">The name of the name.</param>
    /// <returns>An asynchronous enumerable of cache keys.</returns>
    IAsyncEnumerable<string> GetKeys(string name);
}