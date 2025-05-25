using Application.BoundContext.BookAuthoringContext.Queries;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;

namespace Application.BoundContext.BookAuthoringContext.Query.Chapter;

public record GetChapterBySlugQuery(string Slug) : IQuery<ChapterViewModel>;

public class GetChapterBySlugQueryHandler(IBookAuthoringQueries  bookAuthoringQueries)
    : IQueryHandler<GetChapterBySlugQuery, ChapterViewModel>
{
    private readonly IBookAuthoringQueries _bookAuthoringQueries = bookAuthoringQueries;
    public async Task<ChapterViewModel> Handle(GetChapterBySlugQuery request, CancellationToken cancellationToken)
    {
        var chapter = await _bookAuthoringQueries.FindChapterBySlugAsync(request.Slug, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(chapter, "Chương");
        return chapter;
    }
}
