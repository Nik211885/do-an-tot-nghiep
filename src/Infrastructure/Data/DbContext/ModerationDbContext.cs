using Core.Interfaces.Repositories.ModerationContext;
using Infrastructure.Data.Interceptors;
using Infrastructure.Services.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DbContext;

public class ModerationDbContext(DbContextOptions<ModerationDbContext> options,
    IDbConnectionStringSelector dbConnectionStringSelector,
    DispatcherDomainEventInterceptors interceptors) 
    : BaseDbContext(options, dbConnectionStringSelector, interceptors)
{
    
}
