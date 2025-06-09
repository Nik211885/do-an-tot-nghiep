namespace Application.BoundContext.ModerationContext.IntegrationEvent.Event;

public class ApprovalBookIntegrationEvent
    : Models.EventBus.IntegrationEvent
{
    public Guid ChapterId { get; }
    public Guid BookId { get; }
    public Guid ChapterUserId { get; }

    public ApprovalBookIntegrationEvent(Guid chapterId, Guid  chapterUserId, Guid bookId)
    {
        ChapterId = chapterId;
        BookId = bookId;
        ChapterUserId = chapterUserId;
    }
}
