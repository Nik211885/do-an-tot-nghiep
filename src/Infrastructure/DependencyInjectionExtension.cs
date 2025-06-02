using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Application.Interfaces.Notification;
using Application.Interfaces.UnitOfWork;
using Infrastructure.Options;
using Infrastructure.Data.DbContext;
using Infrastructure.Data.Interceptors;
using Infrastructure.Services.Cache;
using Infrastructure.Services.CQRS;
using Infrastructure.Services.DbContext;
using Infrastructure.Services.Elastic;
using Infrastructure.Services.EventBus;
using Infrastructure.Services.Keycloak;
using Infrastructure.Services.Notification;
using Infrastructure.Services.ProcessData;
using Infrastructure.Services.Repository;
using Infrastructure.Services.UnitOfWork;
using Infrastructure.Services.UploadFile;
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
        services.AddDbContext();
        services.AddScoped<IEventDispatcher, EventDispatcher>();
        services.AddScoped<DispatcherDomainEventInterceptors>();
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddScoped<IIdentityProviderServices, KeycloakServices>();
        services.AddSingleton<IDbConnectionStringSelector, DbConnectionStringSelector>();
        services.AddRepository();
        services.AddMassTransitRabbitMqEventBus();
        services.AddApplicationServicesExtension();
        services.AddProcessData();
        services.AddElastic();
        services.AddOptionsExtension(configuration);
        services.AddKeyCloakIdentityProvider();
        services.AddCache();
        services.AddUploadFileWithCloudinary();
        services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
        return services;
    }
}
