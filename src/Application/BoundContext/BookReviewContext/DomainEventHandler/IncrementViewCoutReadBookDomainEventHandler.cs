using Application.Exceptions;
using Application.Interfaces.CQRS;
using Core.Events.BookReviewContext;
using Core.Interfaces.Repositories.BookReviewContext;
using Org.BouncyCastle.Ocsp;

namespace Application.BoundContext.BookReviewContext.DomainEventHandler;

public class IncrementViewCoutReadBookDomainEventHandler(IBookReviewRepository bookReviewRepository)
    : IEventHandler<ReaderBookDomainEvent>
{
    private readonly IBookReviewRepository _bookReviewRepository = bookReviewRepository;

    public async Task Handler(ReaderBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var bookReview =
            await _bookReviewRepository.GetBookReviewByIdAsync(domainEvent.ReaderBook.BookReviewId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(bookReview, "Sách");
        bookReview.IncrementView();
        _bookReviewRepository.UpdateBookReview(bookReview);
        await _bookReviewRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
    }
}

