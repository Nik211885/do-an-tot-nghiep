using System.Linq.Expressions;
using Application.Interfaces.Validator;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Infrastructure.Data.DbContext;
using Infrastructure.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.Services.Validator;

public class ValidationServices<TEntity>
    : IValidationServices<TEntity> where TEntity : class
{
    private readonly BaseDbContext _dbContext;
    // Find db context for type entity
    public ValidationServices(IServiceProvider serviceProvider, IEntityDbContextMapService dbContextMapService)
    {
        var typeDbContext = dbContextMapService.GetDbContextTypeForEntity<TEntity>();
        _dbContext = (BaseDbContext)serviceProvider.GetRequiredService(typeDbContext);
    }

    public async Task<TEntity?> AnyAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken, bool ignoreQueryFilters = true)
    {
        var query = _dbContext.Set<TEntity>()
            .AsNoTracking();
        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }
        var result = await query
            .FirstOrDefaultAsync(selector, cancellationToken);
        return result;

    }

    public async Task<int> CoutAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken, bool ignoreQueryFilters = true)
    {
        var query = _dbContext.Set<TEntity>()
            .AsNoTracking();
        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }
        var result = await query
            .CountAsync(selector, cancellationToken);
        return result;
    }
}
