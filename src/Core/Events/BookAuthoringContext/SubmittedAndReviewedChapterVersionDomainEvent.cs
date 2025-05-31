namespace Core.Events.BookAuthoringContext;

public class SubmittedAndReviewedChapterVersionDomainEvent : IEvent
{
    public SubmittedAndReviewedChapterVersionDomainEvent()
    {
        
    }
    public Guid ChapterVersionId { get; }

    public SubmittedAndReviewedChapterVersionDomainEvent(Guid chapterVersionId)
    {
        ChapterVersionId = chapterVersionId;
    }
}
