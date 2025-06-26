using Core.BoundContext.OrderContext.OrderAggregate;

namespace Application.BoundContext.OrderContext.ViewModel;

public class OrderItemViewModel
{
    public Guid Id { get; }
    public Guid BookId { get;}
    public string BookName { get;  }
    public decimal Price { get;}

    public OrderItemViewModel(Guid id,Guid bookId, string bookName, decimal price)
    {
        Id = id;
        BookId = bookId;
        BookName = bookName;
        Price = price;
    }
}

public static class MappingOrderItemExtension
{
    public static OrderItemViewModel ToViewModel(this OrderItem orderItem)
    {
        return new OrderItemViewModel(orderItem.Id, orderItem.BookId,  orderItem.BookName, orderItem.Price);
    }

    public static IEnumerable<OrderItemViewModel> ToViewModel(this IEnumerable<OrderItem> orderItems)
    {
        return orderItems.Select(ToViewModel);
    }
}
