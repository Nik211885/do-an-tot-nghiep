using Core.BoundContext.BookAuthoringContext.BookAggregate;

namespace Core.Events.BookAuthoringContext;

public class RemovedGenreForBookDomainEvent(Guid bookId, params BookGenres[] bookGenres) : IEvent
{
    public Guid  BookId { get; } = bookId;
    public BookGenres[]  BookGenres { get; } = bookGenres;
}
