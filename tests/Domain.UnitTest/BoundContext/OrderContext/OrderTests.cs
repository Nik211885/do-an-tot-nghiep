using Core.BoundContext.OrderContext.OrderAggregate;

namespace Domain.UnitTest.BoundContext.OrderContext;

public class OrderTests
{
    [Fact]
    public void Create_Should_Succeed_With_Valid_Data()
    {
        var buyerId = Guid.NewGuid();
        var order = Order.Create(buyerId);
        Assert.Equal(buyerId, order.BuyerId);
        Assert.Equal(OrderStatus.Pending, order.Status);
        Assert.Empty(order.OrderItems);
    }

    [Fact]
    public void AddOrderItem_Should_Add_Item()
    {
        var order = Order.Create(Guid.NewGuid());
        var bookId = Guid.NewGuid();
        order.AddOrderItem(bookId, "Book", 100);
        Assert.Single(order.OrderItems);
        Assert.Equal(bookId, order.OrderItems.First().BookId);
    }

    [Fact]
    public void AddOrderItem_Should_Throw_When_Duplicate()
    {
        var order = Order.Create(Guid.NewGuid());
        var bookId = Guid.NewGuid();
        order.AddOrderItem(bookId, "Book", 100);
        var ex = Assert.Throws<Core.Exception.BadRequestException>(() => order.AddOrderItem(bookId, "Book", 100));
        Assert.Contains("OrderHasInCart", ex.Message);
    }

    [Fact]
    public void RemoveOrderItem_Should_Remove_Item()
    {
        var order = Order.Create(Guid.NewGuid());
        var bookId = Guid.NewGuid();
        order.AddOrderItem(bookId, "Book", 100);
        order.RemoveOrderItem(bookId);
        Assert.Empty(order.OrderItems);
    }

    [Fact]
    public void RemoveOrderItem_Should_Throw_When_Not_Exist()
    {
        var order = Order.Create(Guid.NewGuid());
        var ex = Assert.Throws<Core.Exception.BadRequestException>(() => order.RemoveOrderItem(Guid.NewGuid()));
        Assert.Contains("OrderHasInCart", ex.Message);
    }

    [Fact]
    public void CalculatePrice_Should_Return_Sum()
    {
        var order = Order.Create(Guid.NewGuid());
        order.AddOrderItem(Guid.NewGuid(), "Book1", 100);
        order.AddOrderItem(Guid.NewGuid(), "Book2", 200);
        Assert.Equal(300, order.CalculatePrice());
    }

    [Fact]
    public void OrderSuccess_Should_Set_Status_And_RaiseEvent()
    {
        var order = Order.Create(Guid.NewGuid());
        order.OrderSuccess();
        Assert.Equal(OrderStatus.Success, order.Status);
        Assert.NotNull(order.DomainEvents);
    }

    [Fact]
    public void OrderFailed_Should_Set_Status_And_RaiseEvent()
    {
        var order = Order.Create(Guid.NewGuid());
        order.OrderFailed();
        Assert.Equal(OrderStatus.Failure, order.Status);
        Assert.NotNull(order.DomainEvents);
    }

    [Fact]
    public void OrderCancelled_Should_Set_Status_And_RaiseEvent()
    {
        var order = Order.Create(Guid.NewGuid());
        order.OrderCancelled();
        Assert.Equal(OrderStatus.Canceled, order.Status);
        Assert.NotNull(order.DomainEvents);
    }
}
