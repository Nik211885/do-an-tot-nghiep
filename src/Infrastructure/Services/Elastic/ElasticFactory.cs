using System.Collections.Concurrent;
using Application.Interfaces.Elastic;
using Elastic.Clients.Elasticsearch;

namespace Infrastructure.Services.Elastic;

public class ElasticFactory(ElasticsearchClient client) : IElasticFactory
{
    private readonly ElasticsearchClient _client = client;

    // Thread-safe dictionary
    private readonly ConcurrentDictionary<string, object> _repositories = new();

    public IElasticServices<TType> GetInstance<TType>() where TType : class
    {
        string typeName = typeof(TType).Name;

        var instance = _repositories.GetOrAdd(typeName, _ =>
        {
            var repositoryType = typeof(ElasticServices<>).MakeGenericType(typeof(TType));
            return Activator.CreateInstance(repositoryType, _client)!;
        });

        return (IElasticServices<TType>)instance;
    }
}
