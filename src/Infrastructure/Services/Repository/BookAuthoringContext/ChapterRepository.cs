using Core.BoundContext.BookAuthoringContext.ChapterAggregate;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repository.BookAuthoringContext;

public class ChapterRepository(BookAuthoringDbContext bookAuthoringDbContext) 
    : Repository<Chapter>(bookAuthoringDbContext), IChapterRepository
{
    private readonly BookAuthoringDbContext _bookAuthoringDbContext = bookAuthoringDbContext;
    public async Task<Chapter?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var chapterById = await _bookAuthoringDbContext.Chapters
            .Where(chapter => chapter.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (chapterById is not null)
        {
            await _bookAuthoringDbContext.Entry(chapterById)
                .Collection(cv=>cv.ChapterVersions)
                .LoadAsync(cancellationToken);
        }
        return chapterById;
    }

    public Chapter Create(Chapter chapter)
    {
        return _bookAuthoringDbContext.Chapters.Add(chapter).Entity;
    }

    public Chapter Update(Chapter chapter)
    {
        _bookAuthoringDbContext.Entry(chapter).State = EntityState.Modified;
        return chapter;
    }

    public void Delete(Chapter chapter)
    {
        _bookAuthoringDbContext.Chapters.Remove(chapter);
    }
}
