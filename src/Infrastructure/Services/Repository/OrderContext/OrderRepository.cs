using Core.BoundContext.OrderContext.OrderAggregate;
using Core.Interfaces.Repositories.OrderContext;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repository.OrderContext;

public class OrderRepository(OrderDbContext orderDbContext) 
    : Repository<Order>(orderDbContext), IOrderRepository
{
    private readonly OrderDbContext _orderDbContext = orderDbContext;
    public Order Create(Order order)
    {
        return _orderDbContext.Add(order).Entity;
    }

    public Order Update(Order order)
    {
        return _orderDbContext.Update(order).Entity;
    }

    public void Delete(Order order)
    {
        _orderDbContext.Remove(order); 
    }

    public async Task<Order?> GetOrderByCodeAsync(string code, CancellationToken cancellationToken)
    {
        var query = _orderDbContext
            .Orders.AsNoTracking()
            .Include(x=>x.OrderItems)
            .Where(x=>x.OrderCode == code);
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var order = await _orderDbContext.Orders.
            Where(x=>x.Id == id)
            .Include(x=>x.OrderItems)
            .FirstOrDefaultAsync(cancellationToken);
        return order;
    }
}
