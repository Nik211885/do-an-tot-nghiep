using System.Reflection;
using Application.Interfaces.CQRS;
using Core.Interfaces.Repositories;
using Infrastructure.Configurations;
using Infrastructure.Data;
using Infrastructure.Services.CQRS;
using Infrastructure.Services.DbContext;
using Infrastructure.Services.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        // services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));
        // services.AddSingleton<ICache, RedisCache>();
        services.AddScoped<IEventDispatcher, EventDispatcher>();
        services.AddDbContext<ApplicationDbContext>();
        services.AddSingleton<IDbConnectionStringSelector, DbConnectionStringSelector>();
        services.AddRepository(typeof(IRepository<>).Assembly, Assembly.GetExecutingAssembly());
        services.AddOptionConfigurations(configuration);
        return services;
    }
}
