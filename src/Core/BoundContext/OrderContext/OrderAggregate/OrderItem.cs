using Core.Entities;

namespace Core.BoundContext.OrderContext.OrderAggregate;

public class OrderItem : BaseEntity
{
    public Guid BookId { get; private set; }
    public string BookName { get; private set; }
    public decimal Price { get; private set; }
    protected OrderItem(){}
    private OrderItem(Guid bookId, string bookName, decimal price)
    {
        BookId = bookId;
        Price = price;
        BookName = bookName;
    }
    public static OrderItem Create(Guid bookId, string bookName, decimal price)
    {
        return new OrderItem(bookId, bookName, price);
    }
}
