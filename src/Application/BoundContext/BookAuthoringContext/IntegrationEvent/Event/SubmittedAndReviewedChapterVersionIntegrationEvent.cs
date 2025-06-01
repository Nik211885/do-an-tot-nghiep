namespace Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;

public class SubmittedAndReviewedChapterVersionIntegrationEvent
    : Models.EventBus.IntegrationEvent
{
    public Guid ChapterId { get; }
    public string Content { get; }
    public Guid BookId { get; } 
    public Guid AuthorId { get; }
     public SubmittedAndReviewedChapterVersionIntegrationEvent(Guid chapterId, Guid bookId, Guid authorId, string content)
     {
         BookId = bookId;
         AuthorId = authorId;
         Content = content; 
         ChapterId = chapterId;
     }
}
