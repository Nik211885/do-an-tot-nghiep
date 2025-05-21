namespace Core.Events.WriteBookContext;

public class SubmittedAndReviewedChapterVersionDomainEvent(Guid chapterVersionId) : IEvent
{
    public Guid ChapterVersionId { get; } = chapterVersionId;
}
