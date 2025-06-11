using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Options;
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
    internal static IServiceCollection AddOptionsExtension(this IServiceCollection services,
        IConfiguration configuration)
    {
        const string cloudinary = "UploadFile:Cloudinary";
        const string redis = "Cache:RedisConnection";
        const string postgres = "DatabaseConnectionString:Postgresql";
        const string keycloak = "KeyCloakAuthentication:BookStoreServer";
        const string mail = "MailSettings";
        const string rabbitmq = "RabbitMq";
        const string docSign = "DocSign";
        services.Configure<CloudinaryUploadFileOptions>(configuration.GetSection(cloudinary));
        services.Configure<CacheOptions>(configuration.GetSection(redis));
        services.Configure<DatabaseConnectionStringOptions>(configuration.GetSection(postgres));
        services.Configure<KeycloakOptions>(configuration.GetSection(keycloak));
        services.Configure<MailSettingOptions>(configuration.GetSection(mail));
        services.Configure<RabbitMqOptions>(configuration.GetSection(rabbitmq));
        services.Configure<DocSignOption>(configuration.GetSection(docSign));
        return services;
    }
}
