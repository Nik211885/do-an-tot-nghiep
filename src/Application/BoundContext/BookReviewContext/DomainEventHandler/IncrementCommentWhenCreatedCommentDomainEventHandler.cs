using Application.Interfaces.CQRS;
using Core.Events.BookReviewContext;
using Core.Interfaces.Repositories.BookReviewContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookReviewContext.DomainEventHandler;

public class IncrementCommentWhenCreatedCommentDomainEventHandler(
    ILogger<IncrementCommentWhenCreatedCommentDomainEventHandler> logger,
    ICommentRepository commentRepository,
    IBookReviewRepository bookReviewRepository)
    : IEventHandler<CommentCreatedDomainEvent>,
        IEventHandler<ReplyCreatedDomainEvent>
{
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IBookReviewRepository _bookReviewRepository = bookReviewRepository;
    private readonly ILogger<IncrementCommentWhenCreatedCommentDomainEventHandler> _logger = logger;

    public async Task Handler(CommentCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await IncrementCommentAsync(domainEvent.BookReviewId, cancellationToken);
    }

    public async Task Handler(ReplyCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await IncrementCommentAsync(domainEvent.BookReviewId, cancellationToken);
    }

    private async Task IncrementCommentAsync(Guid bookReviewId, CancellationToken cancellationToken)
    {
        var bookReview = await _bookReviewRepository.GetBookReviewByIdAsync(bookReviewId, cancellationToken);
        if (bookReview is null)
        {
            return;
        }
        bookReview.UpdateCommentCount();
        _bookReviewRepository.UpdateBookReview(bookReview);
        await _bookReviewRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
    }
}
