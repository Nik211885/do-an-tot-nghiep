namespace Application.BoundContext.ModerationContext.IntegrationEvent.Event;

public class ApprovedChapterIntegrationEvent
    : Models.EventBus.IntegrationEvent
{
    public Guid BookId { get; }
    public Guid ChapterId { get; }
    public string Note { get; }

    public ApprovedChapterIntegrationEvent(Guid bookId, Guid chapterId, string note)
    {
        BookId = bookId;
        Note = note;
        ChapterId = chapterId;
    }
}
