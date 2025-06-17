using Core.BoundContext.BookAuthoringContext.ChapterAggregate;

namespace Core.Events.BookAuthoringContext;

public class SubmittedAndReviewedChapterVersionDomainEvent : IEvent
{
    public Chapter Chapter { get;  }    

    public SubmittedAndReviewedChapterVersionDomainEvent(Chapter chapter)
    {
        Chapter = chapter;
    }
}
