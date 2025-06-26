using Core.BoundContext.OrderContext.OrderAggregate;

namespace Core.Interfaces.Repositories.OrderContext;

public interface IOrderRepository
    : IRepository<Order>
{
    Order Create(Order order);
    Order Update(Order order);
    void Delete(Order order);
    Task<Order?> GetOrderByCodeAsync(string code, CancellationToken cancellationToken);
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
