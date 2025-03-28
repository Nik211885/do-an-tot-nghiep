using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations;
/// <summary>
/// 
/// </summary>
internal static class AddOptionConfigurationsExtension
{
    /// <summary>
    ///     
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    internal static IServiceCollection AddOptionConfigurations(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<CacheConnectionConfiguration>(configuration.GetSection("Cache:RedisConnection"));
        services.Configure<DatabaseConnectionString>(configuration.GetSection("DatabaseConnectionString:Postgresql"));
        return services;
    }
}
