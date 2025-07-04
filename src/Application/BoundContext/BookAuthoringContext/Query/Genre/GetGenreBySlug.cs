using Application.BoundContext.BookAuthoringContext.Queries;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;

namespace Application.BoundContext.BookAuthoringContext.Query.Genre;

public record GetGenreBySlugQuery(string Slug) : IQuery<GenreViewModel>;

public class GetGenreBySlugQueryHandler(IBookAuthoringQueries bookAuthoringQueries)
    : IQueryHandler<GetGenreBySlugQuery, GenreViewModel>
{
    private readonly IBookAuthoringQueries _bookAuthoringQueries = bookAuthoringQueries;
    public async Task<GenreViewModel> Handle(GetGenreBySlugQuery request, CancellationToken cancellationToken)
    {
        var genre = await _bookAuthoringQueries.FindGenreBySlugAsync(request.Slug, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(genre,"Thể loại");
        return genre;
    }
}
