using Core.BoundContext.BookAuthoringContext.BookAggregate;

namespace Core.Events.BookAuthoringContext;

public class BookUpdatePolicyReaderBookDomainEvent(
    Guid bookId, PolicyReadBook policyReadBook, Guid userId) : IEvent
{
    public Guid UserId { get; } = userId;
    public Guid BookId { get; } = bookId;
    public PolicyReadBook PolicyReadBook { get; } = policyReadBook;
}
