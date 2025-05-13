using Core.BoundContext.OrderContext.OrderAggregate;
using Core.Interfaces.Repositories.OrderContext;
using Infrastructure.Data.DbContext;

namespace Infrastructure.Services.Repository.OrderContext;

public class OrderRepository(OrderDbContext orderDbContext) 
    : Repository<Order>(orderDbContext), IOrderRepository
{
    private readonly OrderDbContext _orderDbContext = orderDbContext;
}
