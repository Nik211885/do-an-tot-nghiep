﻿using Application.Models;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;

namespace Application.Interfaces.Elastic;

public interface IElasticServices<T>  where T : class
{
    // Use large case with object
    Task<T?> GetAsync(object id);

    Task<IEnumerable<T>> ListAsync();

    Task<SearchResponse<T>> ListAsync(
        QueryParamRequest request,
        Action<QueryDescriptor<T>>? filter = null
    );
    Task<PaginationItem<T>> PaginatedListAsync(
        QueryParamRequest request,
        Action<QueryDescriptor<T>>? filter = null
    );

    Task<T> AddAsync(T entity);

    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

    Task UpdateAsync(T entity);

    Task UpdateRangeAsync(IEnumerable<T> entities);

    Task DeleteAsync(T entity);

    Task DeleteRangeAsync(IEnumerable<T> entities);

    Task UpdateByQueryAsync(T entity, string query, Dictionary<string, object> parameters);

    Task DeleteByQueryAsync(Action<QueryDescriptor<T>> querySelector);

    Task<bool> AnyAsync(Action<BoolQueryDescriptor<T>> selector);

    Task<long> CountAsync(CountRequestDescriptor<T> selector);
}
