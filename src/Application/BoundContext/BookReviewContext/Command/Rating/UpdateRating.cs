using Application.BoundContext.BookReviewContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Core.Interfaces.Repositories.BookReviewContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookReviewContext.Command.Rating;

public record UpdateRatingCommand(Guid Id, int Star)
    : IBookReviewCommand<RatingViewModel>;

public class UpdateRatingCommandHandler(IRatingRepository ratingRepository,
    ILogger<UpdateRatingCommandHandler> logger)
    : ICommandHandler<UpdateRatingCommand, RatingViewModel>
{
    private readonly IRatingRepository _ratingRepository = ratingRepository;
    private readonly ILogger<UpdateRatingCommandHandler> _logger = logger;
    
    public async Task<RatingViewModel> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
    {
        var rating = await _ratingRepository.GetRatingByIdAsync(request.Id, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(rating, "Không tìm thấy đánh giá của bạn");
        rating.UpdateRating(request.Star);
        _ratingRepository.UpdateRating(rating);
        _logger.LogInformation("Update rating {@Id} with star {@Star}", request.Id, request.Star);
       await _ratingRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
       return rating.MapToViewModel();
    }
}
