using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Interfaces.Repositories.ModerationContext;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repository.ModerationContext;

public class ChapterApprovalRepository(ModerationDbContext dbContext)
    :Repository<ChapterApproval>(dbContext), IChapterApprovalRepository
{
    private readonly ModerationDbContext _dbContext = dbContext;
    public async Task<ChapterApproval?> FindByChapterIdAsync(Guid chapterId)
    {
        var query = _dbContext.ChapterApprovals
            .Where(x => x.ChapterId == chapterId)
            .Include(x => x.Decision);
        var result = await query.FirstOrDefaultAsync();
        return result;
    }

    public ChapterApproval Create(ChapterApproval chapter)
    {
        return _dbContext.ChapterApprovals.Add(chapter).Entity;
    }

    public ChapterApproval Update(ChapterApproval chapterApproval)
    {
        _dbContext.ChapterApprovals.Entry(chapterApproval).State = EntityState.Modified;
        return chapterApproval;
    }
}
