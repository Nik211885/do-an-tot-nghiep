using Application.Interfaces.ProcessData;
using Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services.ProcessData;

public static class AddProcessDataExtension
{
    public static IServiceCollection AddProcessData(this IServiceCollection services)
    {
        services.AddHttpClient(HttpClientKeyFactory.EmbeddingServer, (collection, client) =>
        {
            var embeddingServerKey = "EmbeddingServer";
            var configuration = collection.GetRequiredService<IConfiguration>();
            var baseUrlEmbeddingServer = configuration.GetValue<string>(embeddingServerKey);
            ArgumentException.ThrowIfNullOrWhiteSpace(baseUrlEmbeddingServer);
            client.BaseAddress = new Uri(baseUrlEmbeddingServer);
        });
        services.AddSingleton<ICleanTextService, CleanTextService>();
        services.AddScoped<IVectorEmbeddingTextService, VectorEmbeddingTextService>();
        return services;
    }
}
