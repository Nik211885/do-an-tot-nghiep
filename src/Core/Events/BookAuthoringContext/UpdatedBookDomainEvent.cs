using Core.BoundContext.BookAuthoringContext.BookAggregate;

namespace Core.Events.BookAuthoringContext;

public class UpdatedBookDomainEvent(Book book)
: IEvent
{
    public Book Book { get; } = book;
}
