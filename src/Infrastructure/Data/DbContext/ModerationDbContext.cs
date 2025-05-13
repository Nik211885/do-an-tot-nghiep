using Infrastructure.Services.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DbContext;

public class ModerationDbContext(DbContextOptions<BaseDbContext> options, IDbConnectionStringSelector dbConnectionStringSelector) 
    : BaseDbContext(options, dbConnectionStringSelector)
{
    
}
