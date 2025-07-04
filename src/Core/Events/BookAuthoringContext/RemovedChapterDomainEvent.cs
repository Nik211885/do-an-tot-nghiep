namespace Core.Events.BookAuthoringContext;

public class RemovedChapterDomainEvent(Guid bookId, Guid chapterId) : IEvent
{
    public Guid BookId { get; } = bookId;
    public Guid ChapterId { get; } = chapterId;
};
