using System.Runtime.InteropServices;
using Application.BoundContext.OrderContext.DomainEventHandler;
using Core.BoundContext.OrderContext.OrderAggregate;

namespace Application.BoundContext.OrderContext.ViewModel;

public class OrderViewModel
{
    public Guid Id { get; }
    public DateTimeOffset OrderDate { get; }
    public Guid BuyerId { get; }
    public OrderStatus Status { get; }
    public IReadOnlyCollection<OrderItemViewModel> OrderItems { get; }

    public OrderViewModel(
        Guid id,DateTimeOffset orderDate,
        Guid buyerId, OrderStatus status,
        IReadOnlyCollection<OrderItemViewModel> orderItems)
    {
        Id = id;
        OrderDate = orderDate;
        BuyerId = buyerId;
        Status = status;
        OrderItems = orderItems;
    }
}

public static class MappingOrderExtension
{
    public static OrderViewModel ToViewModel(this Order order)
    {
        return new OrderViewModel(order.Id,order.OrderDate, 
            order.BuyerId, order.Status,
            order.OrderItems.ToViewModel().ToList());
    } 
}
