using Core.Entities;
using Core.Exception;

namespace Core.BoundContext.OrderContext.OrderAggregate;

public class OrderItem : BaseEntity
{
    public Guid BookId { get; private set; }
    public string BookName { get; private set; }
    public decimal Price { get; private set; }
    public Guid AuthorId { get; private set; }
    protected OrderItem(){}
    private OrderItem(Guid bookId, string bookName, decimal price, Guid authorId)
    {
        BookId = bookId;
        AuthorId = authorId;
        Price = price;
        BookName = bookName;
    }
    public static OrderItem Create(Guid bookId, string bookName, decimal price, Guid authorId)
    {
        return new OrderItem(bookId, bookName, price, authorId);
    }

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice <=0)
        {
            ThrowHelper.ThrowIfBadRequest("So tien khong duoc be hon 0");
        }
        Price = newPrice;
    }
}
