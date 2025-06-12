using System.Linq.Expressions;

namespace Application.Interfaces.Validator;

public interface IValidationServices<TEntity> where TEntity : class
{
    Task<TEntity?> AnyAsync(Expression<Func<TEntity, bool>> selector,
        CancellationToken cancellationToken,
        bool ignoreQueryFilters = true);
    Task<int> CoutAsync(Expression<Func<TEntity, bool>> selector,
        CancellationToken cancellationToken,
        bool ignoreQueryFilters = true);
}
