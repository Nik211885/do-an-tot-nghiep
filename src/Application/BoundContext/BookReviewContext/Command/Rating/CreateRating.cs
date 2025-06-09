using Application.BoundContext.BookReviewContext.Command.BookReview;
using Application.BoundContext.BookReviewContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Application.Interfaces.Validator;
using Core.Interfaces.Repositories.BookReviewContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookReviewContext.Command.Rating;

public record CreateRatingCommand(Guid BookId, int starValue)
    : IBookReviewCommand<RatingViewModel>;

public class CreateRatingCommandHandler(
    ILogger<CreateBookReviewCommandHandler> logger,
    IRatingRepository ratingRepository,
    IFactoryHandler factoryHandler,
    IIdentityProvider identityProvider,
    IValidationServices<Core.BoundContext.BookReviewContext.BookReviewAggregate.BookReview> validationBookReviewServices)
    : ICommandHandler<CreateRatingCommand, RatingViewModel>
{
    private readonly ILogger<CreateBookReviewCommandHandler> _logger = logger;
    private readonly IRatingRepository _ratingRepository = ratingRepository;
    private readonly IFactoryHandler _factoryHandler = factoryHandler;
    private readonly IIdentityProvider _identityProvider = identityProvider;

    private readonly IValidationServices<Core.BoundContext.BookReviewContext.BookReviewAggregate.BookReview>
        _validationBookReviewServices = validationBookReviewServices;
    public async Task<RatingViewModel> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
    {
        var bookReview = await _validationBookReviewServices.AnyAsync(b=>b.BookId ==  request.BookId, cancellationToken);
        var bookReviewViewModel = bookReview?.MapToViewModel(); 
        if (bookReviewViewModel is null)
        {
            _logger.LogInformation("Can't not find book review for book {@BookId} Create new Book review", request.BookId);
            var createBookReviewCommand = new CreateBookReviewCommand(request.BookId);
            bookReviewViewModel = await _factoryHandler.Handler<CreateBookReviewCommand, BookReviewViewModel>(createBookReviewCommand, cancellationToken);
        }
        var rating = Core.BoundContext.BookReviewContext.RatingAggregate.Rating.Create(
            bookReviewId: bookReviewViewModel.BookId,
            reviewerId: _identityProvider.UserIdentity(),
            starValue: request.starValue
        );
        _ratingRepository.CreateRating(rating);
        return rating.MapToViewModel();
    }
}
