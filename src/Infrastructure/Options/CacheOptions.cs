namespace Infrastructure.Options;
/// <summary>
/// 
/// </summary>
[KeyOptions("Cache:RedisConnection")]
public class CacheOptions
{
    public string? Master { get; set; }
    public string[]? Slaves { get; set; }
}

