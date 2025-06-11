using Core.BoundContext.BookAuthoringContext.BookAggregate;

namespace Core.Events.BookAuthoringContext;

public class AddedGenreForBookDomainEvent(Book book,
    Guid id, params BookGenres[] bookGenres) : IEvent
{
    public Guid  BookId { get; } = id;
    public Book Book { get; } = book;
    public BookGenres[] BookGenres { get; } = bookGenres;
}
