using Core.BoundContext.WriteBookContext.ChapterAggregate;
using Core.Interfaces.Repositories.WriteBookContext;
using Infrastructure.Data.DbContext;

namespace Infrastructure.Services.Repository.WriteBookContext;

public class ChapterRepository(WriteBookDbContext writeBookDbContext) 
    : Repository<Chapter>(writeBookDbContext), IChapterRepository
{
    private readonly WriteBookDbContext _writeBookDbContext = writeBookDbContext;
}
