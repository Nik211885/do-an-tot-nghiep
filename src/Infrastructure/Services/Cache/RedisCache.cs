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

    public async Task<string?> GetAsync(string key)
        => await _db.StringGetAsync(key);

    public async Task SetAsync(string key, string value, TimeSpan leftTime)
        => await  _db.StringSetAsync(key, value, leftTime);

    public async Task RemoveAsync(string key)
        => await _db.KeyDeleteAsync(key);
}
