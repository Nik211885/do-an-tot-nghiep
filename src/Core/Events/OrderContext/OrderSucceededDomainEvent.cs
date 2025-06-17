using Core.BoundContext.OrderContext.OrderAggregate;

namespace Core.Events.OrderContext;

public class OrderSucceededDomainEvent(Order order)
    : IEvent
{
    public Order Order { get; } = order;
}
