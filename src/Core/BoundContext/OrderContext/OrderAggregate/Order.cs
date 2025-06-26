using System.Diagnostics.CodeAnalysis;
using Core.Entities;
using Core.Events.OrderContext;
using Core.Exception;
using Core.Interfaces;
using Core.Message;

namespace Core.BoundContext.OrderContext.OrderAggregate;

public class Order : BaseEntity, IAggregateRoot
{
    public DateTimeOffset OrderDate { get; private set; }
    // Buyer for user
    public Guid BuyerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public string OrderCode { get; private set; } = string.Empty;
    private List<OrderItem> _orderItems =[];
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
    protected Order(){}

    private Order(Guid buyerId)
    {
        BuyerId = buyerId;
        Status = OrderStatus.Pending;
        OrderDate = DateTimeOffset.UtcNow;
    }

    public static Order Create(Guid buyerId)
    {
        return new Order(buyerId);
    }

    public void AddOrderItem(Guid bookId, string bookName, decimal price)
    {
        var existingBookInOrders = _orderItems.FirstOrDefault(o => o.BookId == bookId);
        ThrowHelper.ThrowBadRequestWhenArgumentNotNull(existingBookInOrders, OrderContextMessage.OrderHasInCart);
        var orderItems = OrderItem.Create(bookId, bookName, price);
        _orderItems.Add(orderItems);
    }

    public void RemoveOrderItem(Guid bookId)
    {
        var orderItem = _orderItems.FirstOrDefault(o => o.BookId == bookId);
        ThrowHelper.ThrowBadRequestWhenArgumentIsNull(orderItem, OrderContextMessage.OrderHasInCart);
        _orderItems.Remove(orderItem);
    }

    public decimal CalculatePrice()
    {
        return _orderItems.Sum(o => o.Price);
    }

    public void OrderSuccess()
    {
        Status = OrderStatus.Success;
        RaiseDomainEvent(new OrderSucceededDomainEvent(this));
    }

    public void OrderFailed()
    {
        Status = OrderStatus.Failure;
        RaiseDomainEvent(new OrderFailedDomainEvent(this));
    }

    public void OrderCancelled()
    {
        Status = OrderStatus.Canceled;
        RaiseDomainEvent(new OrderCanceledDomainEvent(this));
    }
    public void Payment()
    {
        if (Status == OrderStatus.Success)
        {
            ThrowHelper.ThrowIfBadRequest("Đơn này đã thanh toán rồi");
        }
        OrderCode = Guid.NewGuid().ToString();
    }
    
    public void Delete()
    {
        
    }
}
