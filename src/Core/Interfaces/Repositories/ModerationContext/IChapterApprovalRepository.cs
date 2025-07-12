using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Core.Interfaces.Repositories.ModerationContext;

public interface IChapterApprovalRepository 
    : IRepository<ChapterApproval>
{
    Task<ChapterApproval?> FindByChapterIdAsync(Guid chapterId);
    ChapterApproval Create(ChapterApproval chapter);
    ChapterApproval Update(ChapterApproval chapterApproval);
}
