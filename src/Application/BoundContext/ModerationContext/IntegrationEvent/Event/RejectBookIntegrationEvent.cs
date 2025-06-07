namespace Application.BoundContext.ModerationContext.IntegrationEvent.Event;

public class RejectBookIntegrationEvent : Models.EventBus.IntegrationEvent
{
    public Guid ChapterId { get; }
    public Guid ChapterUserId { get; }
    public string Note { get; }

    public RejectBookIntegrationEvent(Guid chapterId, Guid chapterUserId, string note)
    {
        Note = note;
        ChapterId = chapterId;
        ChapterUserId = chapterUserId;
    }
}
