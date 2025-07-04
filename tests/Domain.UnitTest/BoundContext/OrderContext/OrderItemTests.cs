using Core.BoundContext.OrderContext.OrderAggregate;

namespace Domain.UnitTest.BoundContext.OrderContext;

public class OrderItemTests
{
    [Fact]
    public void Create_Should_Succeed_With_Valid_Data()
    {
        var bookId = Guid.NewGuid();
        var item = OrderItem.Create(bookId, "Book", 100);
        Assert.Equal(bookId, item.BookId);
        Assert.Equal("Book", item.BookName);
        Assert.Equal(100, item.Price);
    }
}
