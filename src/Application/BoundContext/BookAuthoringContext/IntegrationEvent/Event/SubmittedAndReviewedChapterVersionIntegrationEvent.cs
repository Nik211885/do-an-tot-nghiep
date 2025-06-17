namespace Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;

public class SubmittedAndReviewedChapterVersionIntegrationEvent
    : Models.EventBus.IntegrationEvent
{
    public Guid ChapterId { get; }
    public string ChapterTitle { get; }
    
    public int ChapterNumber { get; }
    public string ChapterSlug { get; }
    public string BookTitle { get; }
    public string Content { get; }
    public Guid BookId { get; } 
    public Guid AuthorId { get; }
     public SubmittedAndReviewedChapterVersionIntegrationEvent(Guid chapterId,
         Guid bookId, Guid authorId, string content,
         int chapterNumber, string chapterSlug,
         string chapterTitle, string bookTitle)
     {
         ChapterTitle = chapterTitle;
         BookTitle = bookTitle;
         BookId = bookId;
         ChapterNumber = chapterNumber;
         ChapterSlug = chapterSlug;
         AuthorId = authorId;
         Content = content; 
         ChapterId = chapterId;
     }
}
