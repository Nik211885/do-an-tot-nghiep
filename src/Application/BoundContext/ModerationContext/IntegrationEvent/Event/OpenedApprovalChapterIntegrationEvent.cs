namespace Application.BoundContext.ModerationContext.IntegrationEvent.Event;

public class OpenedApprovalChapterIntegrationEvent
    : Models.EventBus.IntegrationEvent
{
    public Guid BookId { get; }
    public Guid ChapterId { get; }

    public OpenedApprovalChapterIntegrationEvent(Guid bookId, Guid chapterId)
    {
        BookId = bookId;
        ChapterId = chapterId;
    }
}
