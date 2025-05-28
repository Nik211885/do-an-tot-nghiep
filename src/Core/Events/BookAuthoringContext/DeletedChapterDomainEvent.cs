using Core.BoundContext.BookAuthoringContext.ChapterAggregate;

namespace Core.Events.BookAuthoringContext;

public class DeletedChapterDomainEvent(Chapter chapter) : IEvent
{
    public Chapter Chapter { get; } = chapter;
}
