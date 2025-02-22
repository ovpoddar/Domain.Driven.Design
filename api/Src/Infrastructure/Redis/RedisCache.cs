using DDD.Application.Abstractions.Redis;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DDD.Infrastructure.Redis;

internal class RedisCache : IRedisCache
{
    private readonly IDistributedCache _distributedCache;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly SemaphoreSlim _semaphoreSlim;
    private readonly DistributedCacheEntryOptions _defaultOption;

    public RedisCache(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer, IOptions<DistributedCacheEntryOptions> cacheOptions)
    {
        _distributedCache = distributedCache;
        _connectionMultiplexer = connectionMultiplexer;
        _defaultOption = cacheOptions.Value;
        _semaphoreSlim = new(1, 1);
    }

    public async Task<T> GetCacheAsync<T>(string key,
        Func<Task<T>> data,
        CancellationToken cancellationToken,
        DateTimeOffset? absoluteExpiration = null,
        TimeSpan? absoluteExpirationRelativeToNow = null,
        TimeSpan? slidingExpiration = null)
        where T : class
    {
        var catchResponse = await GetCacheAsync<T>(key, cancellationToken);
        if (catchResponse is not null)
            return catchResponse;

        await _semaphoreSlim.WaitAsync(1500, cancellationToken);
        catchResponse = await GetCacheAsync<T>(key, cancellationToken);
        if (catchResponse is not null)
            return catchResponse;

        catchResponse = await data();

        await SetCacheAsync(key, catchResponse, cancellationToken, absoluteExpiration, absoluteExpirationRelativeToNow, slidingExpiration);
        _semaphoreSlim.Release();
        return catchResponse;
    }

    public async Task<T?> GetCacheAsync<T>(string key, CancellationToken cancellationToken) where T : class
    {
        var catchResponses = await _distributedCache.GetStringAsync(key, cancellationToken);
        if (catchResponses is null)
            return null;

        var value = JsonSerializer.Deserialize<T>(catchResponses);
        return value;
    }

    public async IAsyncEnumerable<string> GetKeys(string name)
    {
        var servers = _connectionMultiplexer.GetServers();
        foreach (var server in servers)
        {
            var value = new RedisValue(name);
            await foreach (var key in server.KeysAsync(pattern: value))
                yield return key.ToString();
        }
    }

    public async Task RemoveCacheAsync(string key) =>
            await _distributedCache.RemoveAsync(key);

    public async Task<bool> RemoveCacheByPatternAsync(string pattern)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(pattern, nameof(pattern));

        try
        {
            var result = true;
            var servers = _connectionMultiplexer.GetServers();
            var database = _connectionMultiplexer.GetDatabase();
            foreach (var server in servers)
                await foreach (var key in server.KeysAsync(pattern: new RedisValue(pattern)))
                {
                    var response = await database.KeyDeleteAsync(key);
                    result = result & response;
                }
            return result;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task SetCacheAsync<T>(string key,
        T value,
        CancellationToken cancellationToken,
        DateTimeOffset? absoluteExpiration = null,
        TimeSpan? absoluteExpirationRelativeToNow = null,
        TimeSpan? slidingExpiration = null) where T : class
    {
        var catchValue = JsonSerializer.Serialize(value);
        _defaultOption.AbsoluteExpiration ??= absoluteExpiration;
        _defaultOption.AbsoluteExpirationRelativeToNow ??= absoluteExpirationRelativeToNow;
        _defaultOption.SlidingExpiration ??= slidingExpiration;
        await _distributedCache.SetStringAsync(key.ToString(), catchValue, _defaultOption, cancellationToken);
    }
}
