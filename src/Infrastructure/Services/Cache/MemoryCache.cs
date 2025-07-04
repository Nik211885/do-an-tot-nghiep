using System.Text.Json;
using Application.Interfaces.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services.Cache;

public class MemoryCache(IMemoryCache memoryCache) : ICache
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<string?> GetAsync(string key)
    {
        _ = _memoryCache.TryGetValue(key, out string? value);
        return Task.FromResult(value);
    }

    public Task<TValue?> GetAsync<TValue>(string key)
    {
        _ = _memoryCache.TryGetValue(key, out TValue? value);
        return Task.FromResult(value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expiresIn"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task SetAsync(string key, string value, int expiresIn)
    {
        _memoryCache.Set(key, value, TimeSpan.FromSeconds(expiresIn));
        return Task.CompletedTask;
    }

    public Task SetAsync(string key, object value, int expiresIn)
    {
        _memoryCache.Set(key, value, TimeSpan.FromSeconds(expiresIn));
        return Task.CompletedTask;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task RemoveAsync(string key)
    {
        _memoryCache.Remove(key);
        return Task.CompletedTask;
    }
}
