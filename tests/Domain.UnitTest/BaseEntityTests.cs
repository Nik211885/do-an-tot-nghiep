using Core.Entities;
using Core.Events;
using Xunit;

public class BaseEntityTests
{
    private class TestEntity : BaseEntity { public TestEntity(Guid id) { Id = id; } }
    private class TestEvent : IEvent { }

    [Fact]
    public void RaiseDomainEvent_Should_Add_Event()
    {
        var entity = new TestEntity(Guid.NewGuid());
        var evt = new TestEvent();
        entity.RaiseDomainEvent(evt);
        Assert.Single(entity.DomainEvents);
        Assert.Contains(evt, entity.DomainEvents);
    }

    [Fact]
    public void RemoveDomainEvent_Should_Remove_Event()
    {
        var entity = new TestEntity(Guid.NewGuid());
        var evt = new TestEvent();
        entity.RaiseDomainEvent(evt);
        entity.RemoveDomainEvent(evt);
        Assert.Empty(entity.DomainEvents);
    }

    [Fact]
    public void ClearDomainEvents_Should_Clear_All_Events()
    {
        var entity = new TestEntity(Guid.NewGuid());
        entity.RaiseDomainEvent(new TestEvent());
        entity.RaiseDomainEvent(new TestEvent());
        entity.ClearDomainEvents();
        Assert.Empty(entity.DomainEvents);
    }

    [Fact]
    public void Equals_Should_Return_True_For_Same_Id_And_Type()
    {
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);
        Assert.True(entity1.Equals(entity2));
        Assert.True(entity1 == entity2);
        Assert.False(entity1 != entity2);
    }

    [Fact]
    public void Equals_Should_Return_False_For_Different_Id()
    {
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new TestEntity(Guid.NewGuid());
        Assert.False(entity1.Equals(entity2));
        Assert.False(entity1 == entity2);
        Assert.True(entity1 != entity2);
    }

    [Fact]
    public void Equals_Should_Return_False_For_Different_Type()
    {
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new object();
        Assert.False(entity1.Equals(entity2));
    }
} 