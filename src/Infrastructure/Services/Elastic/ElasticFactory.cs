using Application.Interfaces.Elastic;
using Elastic.Clients.Elasticsearch;

namespace Infrastructure.Services.Elastic;

public class ElasticFactory(ElasticsearchClient client) : IElasticFactory
{
    private readonly ElasticsearchClient _client = client;
    private readonly Dictionary<string, object?> _repositories = [];
    public IElasticServices<TType> GetInstance<TType>() where TType : class
    {
        string type = typeof(TType).Name;
        if (!_repositories.TryGetValue(type, out object? value))
        {
            Type repositoryType = typeof(ElasticsearchClient);
            object? repositoryInstance = Activator.CreateInstance(
                repositoryType.MakeGenericType(typeof(TType)),
                [_client]
            );
            value = repositoryInstance;
            _repositories.Add(type, value);
        }

        return (IElasticServices<TType>)value!;
    }
}
