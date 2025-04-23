using System.Reflection;
using Application.Interfaces.Cache;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.Interfaces.Repositories;
using Infrastructure.Configurations;
using Infrastructure.Data;
using Infrastructure.Services.Cache;
using Infrastructure.Services.CQRS;
using Infrastructure.Services.DbContext;
using Infrastructure.Services.Keycloak;
using Infrastructure.Services.Repository;
using Infrastructure.Services.UploadFile;
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
        services.AddScoped<IEventDispatcher, EventDispatcher>();
        services.AddDbContext<ApplicationDbContext>();
        services.AddScoped<IIdentityProviderServices, KeycloakServices>();
        services.AddSingleton<IDbConnectionStringSelector, DbConnectionStringSelector>();
        services.AddRepository(typeof(IRepository<>).Assembly, Assembly.GetExecutingAssembly());
        services.AddOptionConfigurations(configuration);
        services.AddHttpClientKeyCloak();
        services.AddCache();
        services.AddUploadFileWithCloudinary();
        return services;
    }
}
