namespace Application.Models;

public class PaginationItem<TEntity>(
    IReadOnlyCollection<TEntity> items, 
    int pageNumber, 
    int pageSize, 
    int count)
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
    public int PageSize { get; } = pageSize;
    /// <summary>
    /// 
    /// </summary>

    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// 
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;
}
