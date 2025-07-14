namespace Application.BoundContext.ModerationContext.IntegrationEvent.Event;

public class OpenedApprovalChapterIntegrationEvent
    : Models.EventBus.IntegrationEvent
{
    public Guid BookId { get; }
    public Guid ChapterId { get; }
    public Guid AuthorId { get; }

    public OpenedApprovalChapterIntegrationEvent(Guid bookId, Guid chapterId, Guid authorId)
    {
        BookId = bookId;
        ChapterId = chapterId;
        AuthorId = authorId;
    }
}
