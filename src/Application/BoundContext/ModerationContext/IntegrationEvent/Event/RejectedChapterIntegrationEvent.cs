namespace Application.BoundContext.ModerationContext.IntegrationEvent.Event;

public class RejectedChapterIntegrationEvent
    : Models.EventBus.IntegrationEvent
{
    public Guid BookId { get; }
    public Guid ChapterId { get; }
    public string Note { get; }

    public RejectedChapterIntegrationEvent(Guid bookId, Guid chapterId, string note)
    {
        BookId = bookId;
        ChapterId = chapterId;
        Note = note;
    }
}
