using Application.BoundContext.BookReviewContext.Command.BookReview;
using Application.BoundContext.BookReviewContext.Message;
using Application.BoundContext.BookReviewContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Application.Interfaces.Validator;
using Core.BoundContext.BookReviewContext.ReaderBookAggregate;
using Core.Interfaces.Repositories.BookReviewContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookReviewContext.Command.Rating;

public record CreateRatingCommand(Guid BookId, int starValue)
    : IBookReviewCommand<RatingViewModel>;

public class CreateRatingCommandHandler(
    ILogger<CreateBookReviewCommandHandler> logger,
    IRatingRepository ratingRepository,
    IIdentityProvider identityProvider,
    IValidationServices<Core.BoundContext.BookReviewContext.BookReviewAggregate.BookReview> validationBookReviewServices,
    IValidationServices<ReaderBook> readerBookValidationServices)
    : ICommandHandler<CreateRatingCommand, RatingViewModel>
{
    private readonly ILogger<CreateBookReviewCommandHandler> _logger = logger;
    private readonly IRatingRepository _ratingRepository = ratingRepository;
    private readonly IIdentityProvider _identityProvider = identityProvider;

    private readonly IValidationServices<Core.BoundContext.BookReviewContext.BookReviewAggregate.BookReview>
        _validationBookReviewServices = validationBookReviewServices;
    private readonly IValidationServices<ReaderBook> _readerBookValidationServices = readerBookValidationServices;
    public async Task<RatingViewModel> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
    {
        // I need check user has read book just has rating for book yet
        var bookReview = await _validationBookReviewServices.AnyAsync(b=>b.BookId ==  request.BookId, cancellationToken);
        if (bookReview is null)
        {
            _logger.LogInformation("Can't not find book review for book {@BookId} Create new Book review",
                request.BookId);
            ThrowHelper.ThrowNotFoundWhenItemIsNull(bookReview,"không tìm thấy sách" );
        }

        var reader = await _readerBookValidationServices.AnyAsync(x => x.BookReviewId == bookReview.Id, cancellationToken);
        if (reader is null)
        {
            // In here user need read book just has rating
            ThrowHelper.ThrowIfBadRequest(RatingValidationMessage.CanNotRatingBookBecauseNotYetReader);
            _logger.LogInformation("Can't not find reader");
        }
        var rating = Core.BoundContext.BookReviewContext.RatingAggregate.Rating.Create(
            bookReviewId: bookReview.Id,
            reviewerId: _identityProvider.UserIdentity(),
            starValue: request.starValue
        );
        _ratingRepository.CreateRating(rating);
        return rating.MapToViewModel();
    }
}
