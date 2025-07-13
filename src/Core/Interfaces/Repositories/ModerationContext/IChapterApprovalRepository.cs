using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Core.Interfaces.Repositories.ModerationContext;

public interface IChapterApprovalRepository 
    : IRepository<ChapterApproval>
{
    Task<ChapterApproval?> FindByIdAsync(Guid id);
    Task<ChapterApproval?> FindByChapterIdAsync(Guid chapterId);
    ChapterApproval Create(ChapterApproval chapter);
    void Remove(ChapterApproval chapterApproval);
    ChapterApproval Update(ChapterApproval chapterApproval);
}
