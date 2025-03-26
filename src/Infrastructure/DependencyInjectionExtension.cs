using Application.Interfaces.CQRS;
using Infrastructure.Services.CQRS;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjectionExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IFactoryHandler, FactoryHandler>();
        services.AddScoped<IEventDispatcher, DomainEventDispatcher>();
        return services;
    }
}
