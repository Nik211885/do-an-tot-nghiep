using Core.BoundContext.OrderContext.OrderAggregate;
using Infrastructure.Data.EntityConfigurations.OrderContext;
using Infrastructure.Data.Interceptors;
using Infrastructure.Services.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DbContext;

public class OrderDbContext(DbContextOptions<OrderDbContext> options, 
    IDbConnectionStringSelector dbConnectionStringSelector,DispatcherDomainEventInterceptors interceptors)
    : BaseDbContext(options, dbConnectionStringSelector,interceptors)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderConfiguration())
            .ApplyConfiguration(new OrderItemConfiguration());
    }
}
