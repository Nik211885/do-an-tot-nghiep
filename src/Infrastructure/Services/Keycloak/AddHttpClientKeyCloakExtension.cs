using Application.Interfaces.IdentityProvider;
using Infrastructure.Configurations;
using Infrastructure.Helper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Keycloak;

public static class AddHttpClientKeyCloakExtension
{
    public static IServiceCollection AddHttpClientKeyCloak(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IIdentityProvider, IdentityProvider>();
        services.AddTransient<HttpClientKeyCloakInterceptor>(); 
        services.AddHttpClient(HttpClientKeyFactory.KeyCloak, (servicesProvider, client) =>
        {
            var keyCloakConfiguration = servicesProvider.GetRequiredService<IOptions<KeycloakConfiguration>>().Value;
            client.BaseAddress = new Uri(string.Concat(keyCloakConfiguration.AddressUrl));
            client.Timeout = TimeSpan.FromMinutes(1.5);
        })
        .AddHttpMessageHandler<HttpClientKeyCloakInterceptor>();
        return services;
    }
}
