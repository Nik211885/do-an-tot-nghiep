using Application.BoundContext.BookAuthoringContext.Queries;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;

namespace Application.BoundContext.BookAuthoringContext.Query.Chapter;

public record GetAllChapterVersionByChapterIdQuery(Guid ChapterId)
    : IQuery<ChapterViewModel>;


public class GetAllChapterVersionByChapterIdQueryHandler(
    IBookAuthoringQueries bookAuthoringQueries)
    : IQueryHandler<GetAllChapterVersionByChapterIdQuery, ChapterViewModel>
{
    private readonly IBookAuthoringQueries _bookAuthoringQueries = bookAuthoringQueries;
    public async Task<ChapterViewModel> Handle(GetAllChapterVersionByChapterIdQuery request, CancellationToken cancellationToken)
    {
        var chapter =await _bookAuthoringQueries.FindChapterVersionByChapterIdAsync(request.ChapterId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(chapter, "Chương");
        return chapter;
    }
}
