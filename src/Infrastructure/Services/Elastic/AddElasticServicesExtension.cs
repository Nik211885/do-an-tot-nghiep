using Application.Interfaces.Elastic;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services.Elastic;

public static class AddElasticServicesExtension
{
    public static IServiceCollection AddElastic(this IServiceCollection services)
    {
        services.AddSingleton<ElasticsearchClient>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var uri = config.GetValue<string>("ElasticConfiguration:Uri");
            ArgumentException.ThrowIfNullOrWhiteSpace(uri);
            var settings = new ElasticsearchClientSettings(new Uri(uri));
            var client = new ElasticsearchClient(settings);
            ArgumentNullException.ThrowIfNull(client);
            return client;
        });
        services.AddScoped<IPlagiarismCheckerService, PlagiarismCheckerService>();
        return services;
    }
}
