using System.Linq.Expressions;
using Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Helper;
/// <summary>
/// 
/// </summary>
public static class PaginationHelperWithEfExtension
{
    /// <summary>
    ///     It just supports make create pagination simple it just still return
    ///     entity items because it is in inf layer if you want cast to response type specific
    ///     cast in application layer with method in cast to is extension to
    ///     pagination models in application layer
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="selector"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public static async Task<PaginationItem<TResponse>> CreatePaginationAsync<TEntity, TResponse>(
        this IQueryable<TEntity> queryable,
            Expression<Func<TEntity, TResponse>> selector,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var countItems = await queryable.CountAsync(cancellationToken);
        var items = await queryable
            .Select(selector)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return new PaginationItem<TResponse>(items, countItems, pageNumber, pageSize);
    }
}
