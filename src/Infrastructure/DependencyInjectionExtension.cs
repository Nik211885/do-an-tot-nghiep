using System.Reflection;
using Application.Interfaces.Cache;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Application.Interfaces.Notification;
using Application.Interfaces.UnitOfWork;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Infrastructure.Configurations;
using Infrastructure.Data;
using Infrastructure.Data.DbContext;
using Infrastructure.Services.Cache;
using Infrastructure.Services.CQRS;
using Infrastructure.Services.DbContext;
using Infrastructure.Services.Keycloak;
using Infrastructure.Services.Notification;
using Infrastructure.Services.Repository;
using Infrastructure.Services.UnitOfWork;
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
        services.AddDbContext();
        services.AddScoped<IEventDispatcher, EventDispatcher>();
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddScoped<IIdentityProviderServices, KeycloakServices>();
        services.AddSingleton<IDbConnectionStringSelector, DbConnectionStringSelector>();
        services.AddRepository();
        services.AddApplicationServicesExtension();
        services.AddOptionConfigurations(configuration);
        services.AddKeyCloakIdentityProvider();
        services.AddCache();
        services.AddUploadFileWithCloudinary();
        services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
        return services;
    }
}
