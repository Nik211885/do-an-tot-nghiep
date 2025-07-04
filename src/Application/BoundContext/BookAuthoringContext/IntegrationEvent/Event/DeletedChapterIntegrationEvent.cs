using Core.BoundContext.BookAuthoringContext.ChapterAggregate;

namespace Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;

public class DeletedChapterIntegrationEvent
 : Models.EventBus.IntegrationEvent
{
    public Guid ChapterId { get;  }
    public Guid BookId { get;  }
    public ChapterStatus Status { get;  }

    public DeletedChapterIntegrationEvent(Guid chapterId, Guid bookId, ChapterStatus status)
    {
        ChapterId = chapterId;
        BookId = bookId;
        Status = status;
    }
}
