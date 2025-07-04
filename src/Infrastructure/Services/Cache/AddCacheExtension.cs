using Application.Interfaces.Cache;
using Infrastructure.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Infrastructure.Services.Cache;

public static class AddCacheExtension
{
    public static IServiceCollection AddCache(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var redisConfiguration = serviceProvider.GetRequiredService<IOptions<CacheOptions>>();
        services.AddSingleton <IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfiguration.Value.Master ??
           throw new Exception("Redis not yet configuration with key is [Cache:RedisConnection]")));
        services.AddSingleton<ICache, RedisCache>();
        return services;
    }
}
