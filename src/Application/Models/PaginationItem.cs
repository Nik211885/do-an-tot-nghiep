namespace Application.Models;
/// <summary>
/// 
/// </summary>
/// <param name="items"></param>
/// <param name="count"></param>
/// <param name="pageNumber"></param>
/// <param name="pageSize"></param>
/// <typeparam name="TEntity"></typeparam>
public class PaginationItem<TEntity>(IReadOnlyCollection<TEntity> items, int count, int pageNumber, int pageSize)
{
    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyCollection<TEntity> Items { get; } = items;
    /// <summary>
    /// 
    /// </summary>
    public int PageNumber { get; } = pageNumber;
    /// <summary>
    /// 
    /// </summary>
    public int TotalPages { get; } = (int)Math.Ceiling(count / (double)pageSize);
    /// <summary>
    /// 
    /// </summary>
    public int TotalCount { get; } = count;
    /// <summary>
    /// 
    /// </summary>

    public bool HasPreviousPage => PageNumber > 1;
    /// <summary>
    /// 
    /// </summary>

    public bool HasNextPage => PageNumber < TotalPages;
}
