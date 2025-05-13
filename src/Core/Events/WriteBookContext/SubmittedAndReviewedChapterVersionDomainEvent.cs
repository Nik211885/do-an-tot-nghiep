namespace Core.Events.WriteBookContext;

public class SubmittedAndReviewedChapterVersionDomainEvent(Guid chapterVersionId, Guid bookId) : IEvent
{
    public Guid ChapterVersionId { get; } = chapterVersionId;
    public Guid BookId { get; } = bookId;
}
