using Core.BoundContext.NotificationContext;
using Infrastructure.Data.EntityConfigurations.NotificationContext;
using Infrastructure.Data.Interceptors;
using Infrastructure.Services.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DbContext;

public class NotificationDbContext(DbContextOptions<NotificationDbContext> options, 
    IDbConnectionStringSelector dbConnectionStringSelector, 
    DispatcherDomainEventInterceptors interceptors) 
    : BaseDbContext(options, dbConnectionStringSelector, interceptors)
{
    public DbSet<Notification> Notifications { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new NotificationConfiguration());
    }
}
