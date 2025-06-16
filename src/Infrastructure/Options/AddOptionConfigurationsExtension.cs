using System.Reflection;
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
        // magic way
        var method = typeof(OptionsConfigurationServiceCollectionExtensions)
            .GetMethods()
            .First(m=>m.Name == nameof(OptionsConfigurationServiceCollectionExtensions.Configure)
                      && m.GetGenericArguments().Length == 1
                      && m.GetParameters().Length == 2
                      && m.GetParameters()[1].ParameterType == typeof(IConfiguration));
        var configurationOptions = Assembly
            .GetExecutingAssembly()
            .GetTypeOptions();
        foreach (var (key, type) in configurationOptions)
        {
            var genericMethod = method.MakeGenericMethod(type);
            var section = configuration.GetSection(key);
            genericMethod.Invoke(null,  [ services,  section ]);
        }
        
        /*services.Configure<CloudinaryUploadFileOptions>(configuration.GetSection(cloudinary));
        services.Configure<CacheOptions>(configuration.GetSection(redis));
        services.Configure<DatabaseConnectionStringOptions>(configuration.GetSection(postgres));
        services.Configure<KeycloakOptions>(configuration.GetSection(keycloak));
        services.Configure<MailSettingOptions>(configuration.GetSection(mail));
        services.Configure<RabbitMqOptions>(configuration.GetSection(rabbitmq));
        services.Configure<DocSignOption>(configuration.GetSection(docSign));*/
        return services;
    }
}
