using Core.BoundContext.OrderContext.OrderAggregate;

namespace Core.Events.OrderContext;

public class OrderCanceledDomainEvent(Order order)
    : IEvent
{
    public Order Order { get; } = order;
}
