using Application.Interfaces.CQRS;
using Core.Events.BookReviewContext;
using Core.Interfaces.Repositories.BookReviewContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookReviewContext.DomainEventHandler;

public class IncrementCommentCreatedReplyDomainEventHandler(
    ILogger<IncrementCommentCreatedReplyDomainEventHandler> logger,
    ICommentRepository commentRepository)
    : IEventHandler<ReplyCreatedDomainEvent>
{
    private readonly ILogger<IncrementCommentCreatedReplyDomainEventHandler> _logger = logger;
    private readonly ICommentRepository _commentRepository = commentRepository;

    public async Task Handler(ReplyCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var parentComment = await _commentRepository.GetCommentByIdAsync(domainEvent.ParentCommentId, cancellationToken);
        if (parentComment is null)
        {
            return;
        }

        parentComment.IncrementReplyCount();
        _commentRepository.UpdateComment(parentComment);
        await _commentRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
    }
}
