using Core.BoundContext.OrderContext.BuyerAggregate;
using Core.BoundContext.OrderContext.OrderAggregate;
using Core.Interfaces.Repositories.OrderContext;
using Infrastructure.Services.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DbContext;

public class OrderDbContext(DbContextOptions<OrderDbContext> options, 
    IDbConnectionStringSelector dbConnectionStringSelector)
    : BaseDbContext(options, dbConnectionStringSelector)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<PaymentMethod> Payments { get; set; }
    public DbSet<Buyer> Buyers { get; set; }
    public DbSet<CardType> CardTypes { get; set; }
}
