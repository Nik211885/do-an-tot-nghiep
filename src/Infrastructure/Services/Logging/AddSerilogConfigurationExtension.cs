using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

namespace Infrastructure.Services.Logging;

public static class AddSerilogConfigurationExtension
{
    public static IServiceCollection AddSerilogConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console(new ElasticsearchJsonFormatter())
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration.GetValue<string>("ElasticSearch") 
                ?? throw new Exception("Can't connect to elasticsearch.")))
            {
                // ModifyConnectionSettings = config => config.BasicAuthentication("",""),
                AutoRegisterTemplate = true,
                IndexFormat = "log-dotnet-{dd-MMM-yyyy HH:mm:ss}",
                CustomFormatter = new ElasticsearchJsonFormatter(renderMessage: true)
            })
            .CreateLogger();
        return services;
    }
}
