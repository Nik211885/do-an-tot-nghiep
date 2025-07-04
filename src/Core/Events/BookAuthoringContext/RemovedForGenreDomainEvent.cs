using Core.BoundContext.BookAuthoringContext.BookAggregate;

namespace Core.Events.BookAuthoringContext;

public class RemovedGenreForBookDomainEvent( Book book,
    Guid bookId, params BookGenres[] bookGenres) : IEvent
{
    public Book Book { get; } = book;
    public Guid  BookId { get; } = bookId;
    public BookGenres[]  BookGenres { get; } = bookGenres;
}
