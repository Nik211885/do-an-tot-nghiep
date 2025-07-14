using Core.Events;

namespace Application.BoundContext.ModerationContext.IntegrationEvent.Event;

public class ApprovedChapterIntegrationEvent
    : Models.EventBus.IntegrationEvent, IEvent
{
    public Guid BookId { get; }
    public Guid ChapterId { get; }
    public string Note { get; }
    public Guid AuthorId { get; }

    public ApprovedChapterIntegrationEvent(Guid bookId, Guid chapterId, string note, Guid authorId)
    {
        BookId = bookId;
        Note = note;
        AuthorId = authorId;
        ChapterId = chapterId;
    }
}
