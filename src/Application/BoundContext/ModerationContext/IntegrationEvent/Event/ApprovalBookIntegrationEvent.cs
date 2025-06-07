namespace Application.BoundContext.ModerationContext.IntegrationEvent.Event;

public class ApprovalBookIntegrationEvent
    : Models.EventBus.IntegrationEvent
{
    public Guid ChapterId { get; }
    public Guid ChapterUserId { get; }

    public ApprovalBookIntegrationEvent(Guid chapterId, Guid  chapterUserId)
    {
        ChapterId = chapterId;
        ChapterUserId = chapterUserId;
    }
}
