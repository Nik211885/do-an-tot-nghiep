using Core.BoundContext.BookAuthoringContext.BookAggregate;

namespace Core.Events.BookAuthoringContext;

public class BookUpdatePolicyReaderBookDomainEvent(
    Book book,
    Guid bookId, PolicyReadBook policyReadBook, Guid userId) : IEvent
{
    public Book Book { get; } = book;
    public Guid UserId { get; } = userId;
    public Guid BookId { get; } = bookId;
    public PolicyReadBook PolicyReadBook { get; } = policyReadBook;
}
