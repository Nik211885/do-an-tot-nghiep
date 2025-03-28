using Application.Interfaces.Cache;
using Application.Interfaces.CQRS;
using Core.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Services.Cache;
using Infrastructure.Services.Repository;
using Infrastructure.Services.CQRS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure;

public static class DependencyInjectionExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFactoryHandler, FactoryHandler>();
        services.AddSingleton<IConnectionMultiplexer>();
        services.AddSingleton<ICache, RedisCache>();
        services.AddScoped<IEventDispatcher, EventDispatcher>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddRepository();
        services.AddOptionConfigurations(configuration);
        return services;
    }
}
