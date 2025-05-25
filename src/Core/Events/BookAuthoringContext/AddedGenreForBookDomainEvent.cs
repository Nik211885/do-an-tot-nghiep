using Core.BoundContext.BookAuthoringContext.BookAggregate;

namespace Core.Events.BookAuthoringContext;

public class AddedGenreForBookDomainEvent(Guid id, params BookGenres[] bookGenres) : IEvent
{
    public Guid  BookId { get; } = id;
    public BookGenres[] BookGenres { get; } = bookGenres;
}
