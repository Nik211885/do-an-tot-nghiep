﻿using Application.Interfaces.Cache;
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
        var isValue = _memoryCache.TryGetValue(key, out string? value);
        return Task.FromResult(value);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="leftTime"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task SetAsync(string key, string value, TimeSpan leftTime)
    {
        _memoryCache.Set(key, value, leftTime);
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
