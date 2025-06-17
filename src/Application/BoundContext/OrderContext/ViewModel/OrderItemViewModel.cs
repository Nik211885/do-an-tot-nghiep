using Core.BoundContext.OrderContext.OrderAggregate;

namespace Application.BoundContext.OrderContext.ViewModel;

public class OrderItemViewModel
{
    public Guid BookId { get;}
    public string BookName { get;  }
    public decimal Price { get;}

    public OrderItemViewModel(Guid bookId, string bookName, decimal price)
    {
        BookId = bookId;
        BookName = bookName;
        Price = price;
    }
}

public static class MappingOrderItemExtension
{
    public static OrderItemViewModel ToViewModel(this OrderItem orderItem)
    {
        return new OrderItemViewModel(orderItem.BookId,  orderItem.BookName, orderItem.Price);
    }

    public static IEnumerable<OrderItemViewModel> ToViewModel(this IEnumerable<OrderItem> orderItems)
    {
        return orderItems.Select(ToViewModel);
    }
}
