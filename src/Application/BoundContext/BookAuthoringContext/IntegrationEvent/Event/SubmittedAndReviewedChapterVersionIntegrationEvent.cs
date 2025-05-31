namespace Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;

public class SubmittedAndReviewedChapterVersionIntegrationEvent
    : Models.EventBus.IntegrationEvent
{
    public Guid ChapterVersionId { get; }

     public SubmittedAndReviewedChapterVersionIntegrationEvent(Guid chapterVersionId)
     {
         ChapterVersionId = chapterVersionId;
     }
}
