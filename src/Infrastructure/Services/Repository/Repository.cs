using Core.Interfaces;
using Core.Interfaces.Repositories;
using Infrastructure.Data.DbContext;

namespace Infrastructure.Services.Repository;

public abstract class Repository<T>(BaseDbContext dbContext) 
    : IRepository<T> where T : IAggregateRoot
{
    /// <summary>
    /// 
    /// </summary>
    private readonly BaseDbContext _dbContext = dbContext;
    /// <summary>
    /// 
    /// </summary>
    public IUnitOfWork UnitOfWork => _dbContext;
    
}
