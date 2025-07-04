using Core.ValueObjects;

namespace Core.BoundContext.BookAuthoringContext.BookAggregate;
public class BookGenres
{
    public Guid GenreId { get; private set; }
    protected BookGenres(){}
    private BookGenres(Guid genreId)
    {
        GenreId = genreId;
    }
    public static BookGenres Create(Guid genreId)
    {
        return new BookGenres(genreId);
    }
}
