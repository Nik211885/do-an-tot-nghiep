using Core.BoundContext.BookAuthoringContext.BookAggregate;

namespace Core.Events.BookAuthoringContext;

public class BookChangedReleaseTypeDomainEvent(Book book,
    Guid bookId, 
    BookReleaseType releaseType, Guid userId) : IEvent
{
    public Book Book { get; } = book;
    public Guid UserId { get; } = userId;
    public Guid BookId { get; } = bookId;
    public BookReleaseType  ReleaseType { get; } = releaseType;
}
