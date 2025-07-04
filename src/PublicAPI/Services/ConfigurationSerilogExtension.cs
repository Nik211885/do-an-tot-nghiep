using System.Reflection;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace PublicAPI.Services;

public static class ConfigurationSerilogExtension
{
    public static IServiceCollection AddConfigurationSerilog(this IServiceCollection services,
        Assembly assembly,
        IConfiguration configuration)
    {
        Serilog.Debugging.SelfLog.Enable(Console.Error);
        var aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                                    ?? throw new Exception("ASPNETCORE_ENVIRONMENT environment variable not set");
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Debug()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(
                new Uri(configuration["ElasticConfiguration:Uri"]
                    ?? throw new Exception("ElasticConfiguration:Uri not set in configuration")))
            {
                AutoRegisterTemplate = true,
                IndexFormat = @$"{assembly.GetName().Name?.ToLower()}-{aspNetCoreEnvironment.ToLower()}-{DateTimeOffset.UtcNow:dd-MM-yyyy}",
                NumberOfReplicas = 1,
                NumberOfShards = 2,
                // ModifyConnectionSettings = con => con.BasicAuthentication(
                //         username: configuration["ElasticConfiguration:Username"] 
                //                   ?? throw new Exception("ElasticConfiguration:Username not set in configuration"),
                //         password: configuration["ElasticConfiguration:Password"] 
                //                   ?? throw new Exception("ElasticConfiguration:Password not set in configuration"))
            })
            .Enrich.WithProperty("Environment", aspNetCoreEnvironment)
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        return services;
    }
}
