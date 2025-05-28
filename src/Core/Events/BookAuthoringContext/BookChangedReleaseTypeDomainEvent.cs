using Core.BoundContext.BookAuthoringContext.BookAggregate;

namespace Core.Events.BookAuthoringContext;

public class BookChangedReleaseTypeDomainEvent(Guid bookId, 
    BookReleaseType releaseType, Guid userId) : IEvent
{
    public Guid UserId { get; } = userId;
    public Guid BookId { get; } = bookId;
    public BookReleaseType  ReleaseType { get; } = releaseType;
}
