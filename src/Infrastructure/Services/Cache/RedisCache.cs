using Application.Interfaces.Cache;
using StackExchange.Redis;

namespace Infrastructure.Services.Cache;
/// <summary>
/// 
/// </summary>
/// <param name="connectionMultiplexer"></param>
public class RedisCache(IConnectionMultiplexer connectionMultiplexer) : ICache
{
    
    private readonly IDatabase _db = connectionMultiplexer.GetDatabase();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<string?> GetAsync(string key)
        => await _db.StringGetAsync(key);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expiresIn"></param>
    public async Task SetAsync(string key, string value, int expiresIn)
        => await  _db.StringSetAsync(key, value, TimeSpan.FromSeconds(expiresIn));
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    public async Task RemoveAsync(string key)
        => await _db.KeyDeleteAsync(key);
}
