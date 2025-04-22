using Core.Models;
using Riok.Mapperly.Abstractions;

namespace Application.Helper;

public static class PaginationItemExtension
{
    /// <summary>
    ///     Map pagination in repository in to want to use want 
    /// </summary>
    /// <param name="paginationItem"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public static PaginationItem<TResponse> MapTo<TEntity, TResponse>(this PaginationItem<TEntity> paginationItem)
        => new PaginationItem<TResponse>(paginationItem.Items.Select(PaginationItemMapper.MapTo<TEntity, TResponse>).ToList(), paginationItem.PageNumber, paginationItem.Items.Count, paginationItem.TotalCount);
}

[Mapper]
public static partial class PaginationItemMapper
{
    public static partial TResponse MapTo<TEntity,TResponse>(TEntity entities);
}
