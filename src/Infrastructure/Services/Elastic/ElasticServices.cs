using Application.Interfaces.Elastic;
using Application.Models;
using CaseConverter;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Fluent;
using Elastic.Clients.Elasticsearch.QueryDsl;
using MailKit.Search;
using SharedKernel.Models;
using SortOrder = Elastic.Clients.Elasticsearch.SortOrder;

namespace Infrastructure.Services.Elastic;

public class ElasticServices<T>(ElasticsearchClient client)
    : IElasticServices<T> where T : class
{
    private readonly ElasticsearchClient _client = client;
    // You can add prefix for index name
    private readonly string _indexName = typeof(T).Name.ToKebabCase();

   public async Task<T> AddAsync(T entity)
    {
        await _client.IndexAsync(entity, i => i.Refresh(Refresh.WaitFor)
            .Index(_indexName));

        return entity;
    }
    
    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
    {
        await _client.BulkAsync(x =>
            x.Index(_indexName).IndexMany(entities).Refresh(Refresh.WaitFor)
        );

        return entities;
    }

    public async Task<bool> AnyAsync(Action<BoolQueryDescriptor<T>> selector)
    {
        SearchResponse<T> searchResponse = await _client.SearchAsync<T>(s =>
            s.Query(q => q.Bool(selector)).Index(_indexName)
        );

        return searchResponse.Documents.Count != 0;
    }

    public async Task<long> CountAsync(CountRequestDescriptor<T> selector)
    {
        CountResponse countResponse = await _client.CountAsync<T>(_ =>
            selector.Indices(_indexName)
        );
        return countResponse.Count;
    }

    public async Task DeleteAsync(T entity)
    {
        await _client.DeleteAsync(entity, i => i.Index(_indexName).Refresh(Refresh.WaitFor));
    }

    public async Task DeleteByQueryAsync(Action<QueryDescriptor<T>> querySelector)
    {
        await _client.DeleteByQueryAsync<T>(x => x.Indices(_indexName).Query(querySelector));
    }

    public async Task DeleteRangeAsync(IEnumerable<T> entities)
    {
        await _client.BulkAsync(x =>
            x.Index(_indexName).DeleteMany(entities).Refresh(Refresh.WaitFor)
        );
    }

    public async Task<T?> GetAsync(object id)
    {
        GetResponse<T> getResponse = await _client.GetAsync<T>(
            id.ToString()!,
            idx => idx.Index(_indexName)
        );

        return getResponse.Source;
    }

    public async Task<IEnumerable<T>> ListAsync()
    {
        SearchResponse<T> searchResponse = await _client.SearchAsync<T>(s =>
            s.Index(_indexName)
        );

        return searchResponse.Documents;
    }

    public async Task<SearchResponse<T>> ListAsync(
        QueryParamRequest request,
        Action<QueryDescriptor<T>>? filter = null
    ) => await SearchAsync(request, filter);

    public async Task<PaginationItem<T>> PaginatedListAsync(
        QueryParamRequest request,
        Action<QueryDescriptor<T>>? filter = null
    )
    {
        SearchResponse<T> searchResponse = await SearchAsync(request, filter);
        return new PaginationItem<T>(
            searchResponse.Documents.AsEnumerable().ToList(),
            request.Page,
            request.PageSize,
            (int)searchResponse.Total
        );
    }

    public async Task UpdateAsync(T entity)
    {
        await _client.UpdateAsync<T, T>(
            entity,
            i => i.Doc(entity).Index(_indexName).Refresh(Refresh.WaitFor)
        );
    }

    public async Task UpdateByQueryAsync(
        T entity,
        string query,
        Dictionary<string, object> parameters
    )
    {
        await _client.UpdateAsync<T, T>(
            entity,
            u =>
                u.Index(_indexName)
                    .Script(s =>
                        s.Source(query)
                            .Params(_ => 
                                new FluentDictionary<string, object>(parameters))
                    )
                    .Refresh(Refresh.True)
        );
    }

    public async Task UpdateRangeAsync(IEnumerable<T> entities)
    {
        await _client.BulkAsync(x =>
            x.Index(_indexName).UpdateMany(entities, (b, i) => b.Doc(i)).Refresh(Refresh.WaitFor)
        );
    }

    private async Task<SearchResponse<T>> SearchAsync(
        QueryParamRequest request,
        Action<QueryDescriptor<T>>? filter = null
    )
    {
        List<Action<QueryDescriptor<T>>> queries = [];
        if (filter != null)
        {
            queries.Add(filter);
        }
        
        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            queries.Add(search =>
                search.Search(request.Keyword, searchProperties: request.Targets)
            );
        }

        void Search(SearchRequestDescriptor<T> search)
        {
            SearchRequestDescriptor<T> _ = new();
            if (queries.Count > 0)
            {
                search.Query(q => q.Bool(b => b.Must(queries.ToArray())));
            }

            search
                .Index(_indexName);
            if (!string.IsNullOrWhiteSpace(request.Sort))
            {
                search.OrderBy(request.Sort.Trim());
            }
            else{
                search.Sort(s => s.Score(x=>x.Order(SortOrder.Desc)));
            }
            search
                .From((request.Page - 1) * request.PageSize)
                .Size(request.PageSize);
        }

        return await _client.SearchAsync<T>(Search);
    }
}

