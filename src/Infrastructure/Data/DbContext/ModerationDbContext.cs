using Infrastructure.Services.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DbContext;

public class ModerationDbContext(DbContextOptions<ModerationDbContext> options, IDbConnectionStringSelector dbConnectionStringSelector) 
    : BaseDbContext(options, dbConnectionStringSelector)
{
    
}
