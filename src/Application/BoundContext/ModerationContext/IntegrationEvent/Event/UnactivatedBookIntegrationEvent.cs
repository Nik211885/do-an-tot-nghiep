﻿namespace Application.BoundContext.ModerationContext.IntegrationEvent.Event;

public class UnactivatedBookIntegrationEvent
    : Models.EventBus.IntegrationEvent
{
    public Guid BookId { get; }
    public Guid AuthorId { get; }

    public UnactivatedBookIntegrationEvent(Guid bookId, Guid authorId)
    {
        BookId = bookId;
        AuthorId = authorId;
    }
}
