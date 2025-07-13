using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Core.Events.ModerationContext;

public class CreatedChapterApprovalDomainEvent(ChapterApproval chapterApproval)
    : IEvent
{
    public ChapterApproval ChapterApproval { get; } = chapterApproval;
}
