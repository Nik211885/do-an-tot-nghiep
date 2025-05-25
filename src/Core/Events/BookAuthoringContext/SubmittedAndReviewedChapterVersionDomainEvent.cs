namespace Core.Events.BookAuthoringContext;

public class SubmittedAndReviewedChapterVersionDomainEvent(Guid chapterVersionId) : IEvent
{
    public Guid ChapterVersionId { get; } = chapterVersionId;
}
