using Core.BoundContext.BookAuthoringContext.BookAggregate;

namespace Core.Events.BookAuthoringContext;

public class CreatedBookDomainEvent(Book book, Guid userId) 
    : IEvent
{
    public Guid UserId { get; } = userId;
    public Book Book { get; } = book;
}
