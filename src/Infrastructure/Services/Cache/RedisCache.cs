using System.Text.Json;
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

    public Task<TValue?> GetAsync<TValue>(string key)
    {
        var value = _db.StringGet(key);
        if (value.IsNullOrEmpty)
        {
            return Task.FromResult(default(TValue));
        }
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        var valueObj = JsonSerializer.Deserialize<TValue>(value!,options);
        return Task.FromResult(valueObj);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expiresIn"></param>
    public async Task SetAsync(string key, string value, int expiresIn)
        => await  _db.StringSetAsync(key, value, TimeSpan.FromSeconds(expiresIn));

    public async Task SetAsync(string key, object value, int expiresIn)
    {
        var jsonObject = JsonSerializer.Serialize(value);
        await _db.StringSetAsync(key, jsonObject, TimeSpan.FromSeconds(expiresIn));
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    public async Task RemoveAsync(string key)
        => await _db.KeyDeleteAsync(key);
}
