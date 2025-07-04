using Application.Interfaces.Clients;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Application.Interfaces.Notification;
using Application.Interfaces.Signature;
using Application.Interfaces.UnitOfWork;
using Application.Interfaces.Validator;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Infrastructure.Options;
using Infrastructure.Data.DbContext;
using Infrastructure.Data.Interceptors;
using Infrastructure.Data.Services;
using Infrastructure.Services.Cache;
using Infrastructure.Services.Clients;
using Infrastructure.Services.CQRS;
using Infrastructure.Services.DbContext;
using Infrastructure.Services.Elastic;
using Infrastructure.Services.EventBus;
using Infrastructure.Services.Keycloak;
using Infrastructure.Services.Notification;
using Infrastructure.Services.Payment;
using Infrastructure.Services.ProcessData;
using Infrastructure.Services.Repository;
using Infrastructure.Services.Signature;
using Infrastructure.Services.UnitOfWork;
using Infrastructure.Services.UploadFile;
using Infrastructure.Services.Validator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MimeKit.Cryptography;

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
        services.AddOptionsExtension(configuration);
        services.AddScoped<IFactoryHandler, FactoryHandler>();
        services.AddDbContext();
        services.AddSingleton<ICheckClientAddressAppServices, CheckClientAddressAppServices>();
        services.AddScoped<IDigitalSignatureService,DigitalSignatureService>();
        services.AddScoped<IEventDispatcher, EventDispatcher>();
        services.AddScoped<DispatcherDomainEventInterceptors>();
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddScoped<IIdentityProviderServices, KeycloakServices>();
        services.AddSingleton<IDbConnectionStringSelector, DbConnectionStringSelector>();
        services.AddRepository();
        services.AddSingleton<IEntityDbContextMapService, EntityDbContextMapService>();
        services.AddScoped(typeof(IValidationServices<>), typeof(ValidationServices<>));
        services.AddMassTransitRabbitMqEventBus();
        services.AddApplicationServicesExtension();
        services.AddProcessData();
        services.AddElastic();
        services.AddPaymentService();
        services.AddHttpContextAccessor();
        services.AddKeyCloakIdentityProvider();
        services.AddCache();
        services.AddUploadFileWithCloudinary();
        services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
        return services;
    }
}
