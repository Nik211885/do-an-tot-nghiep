using Core.BoundContext.BookAuthoringContext.GenresAggregate;
using Core.ValueObjects;

namespace Core.BoundContext.BookAuthoringContext.BookAggregate;

public class BookGenres : ValueObject
{
    public Guid BookId { get; private set; }
    public Book Book { get; private set; }
    public Guid GenreId { get; private set; }
    public Genres Genre { get; private set; }

    private BookGenres(Guid bookId, Guid genreId)
    {
        BookId = bookId;
        GenreId = genreId;
    }

    public static BookGenres Create(Guid bookId, Guid genreId)
    {
        return new BookGenres(bookId, genreId);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return BookId;
        yield return GenreId;
    }
}
