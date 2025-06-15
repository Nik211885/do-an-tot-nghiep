using Application.Models;

namespace Application.Helper;

public static class ObjectExtension
{
    public static PaginationItem<TEntity> CreatePagination<TEntity>
        (this IReadOnlyCollection<TEntity> entities, PaginationRequest page, int cout)
    {
        return new PaginationItem<TEntity>(entities, page.PageNumber, page.PageSize, cout );
    }
}
