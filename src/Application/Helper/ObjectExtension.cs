using Application.Models;

namespace Application.Helper;

public static class ObjectExtension
{
    public static PaginationItem<TEntity> CreatePagination<TEntity>
        (this IReadOnlyCollection<TEntity> entities, PaginationRequest page, int cout)
    {
        return new PaginationItem<TEntity>(entities, page.PageNumber, page.PageSize, cout );
    }
    public static PaginationItem<TResponse> 
        ConvertDataWithPagination<TEntity, TResponse>(PaginationItem<TEntity>
            paginationItem,
            Func<TEntity, TResponse> converter)
    {
        return new PaginationItem<TResponse>(
            paginationItem.Items.Select(converter).ToList(), 
            paginationItem.PageNumber, 
            paginationItem.PageNumber,
            paginationItem.TotalCount);
    }

    public static T GetById<T>(this IEnumerable<T> source, object id)
    {
        var type = typeof(T);
        var idScan = type
            .GetProperties()
            .FirstOrDefault(x => x.Name.ToLower() == "id")
            ?? throw new Exception("Not find key for source");
        var result = source.FirstOrDefault(x => idScan.GetValue(x) == id);
        return result
            ?? throw new Exception("Not find key for source");
    }
}
