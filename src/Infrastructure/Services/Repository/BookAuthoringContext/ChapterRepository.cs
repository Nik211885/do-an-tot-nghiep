using Core.BoundContext.BookAuthoringContext.ChapterAggregate;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Infrastructure.Data.DbContext;

namespace Infrastructure.Services.Repository.BookAuthoringContext;

public class ChapterRepository(BookAuthoringDbContext bookAuthoringDbContext) 
    : Repository<Chapter>(bookAuthoringDbContext), IChapterRepository
{
    private readonly BookAuthoringDbContext _bookAuthoringDbContext = bookAuthoringDbContext;
}
