using Core.BoundContext.BookAuthoringContext.BookAggregate;

namespace Core.Events.BookAuthoringContext;

public class CreatedBookDomainEvent(Book book) 
    : IEvent
{
    public Book Book { get; } = book;
}
