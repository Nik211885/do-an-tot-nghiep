using Application.BoundContext.BookReviewContext.Command.BookReview;
using Application.BoundContext.BookReviewContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Application.Interfaces.Validator;
using Core.Interfaces.Repositories.BookReviewContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookReviewContext.Command.Comment;

public record CreateCommentCommand(Guid BookId, string Content,
    Guid? ParentCommentId) 
    : IBookReviewCommand<CommentViewModel>;


public class CreateCommentCommandHandler(
    ILogger<CreateBookReviewCommandHandler> logger,
    ICommentRepository commentRepository,
    IIdentityProvider identityProvider,
    IValidationServices<Core.BoundContext.BookReviewContext.BookReviewAggregate.BookReview> validationBookReviewServices,
    IValidationServices<Core.BoundContext.BookReviewContext.CommentAggregate.Comment> validationCommentServices)
    : ICommandHandler<CreateCommentCommand, CommentViewModel>
{
    private readonly ILogger<CreateBookReviewCommandHandler> _logger = logger;
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IIdentityProvider _identityProvider = identityProvider;

    private readonly IValidationServices<Core.BoundContext.BookReviewContext.BookReviewAggregate.BookReview>
        _validationBookReviewServices = validationBookReviewServices;
    private readonly IValidationServices<Core.BoundContext.BookReviewContext.CommentAggregate.Comment>
        _validationCommentServices = validationCommentServices;
    public async Task<CommentViewModel> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var bookReview = await _validationBookReviewServices.AnyAsync(b=>b.BookId ==  request.BookId, cancellationToken);
        var bookReviewViewModel = bookReview?.MapToViewModel(); 
        if (bookReviewViewModel is null)
        {
            _logger.LogInformation("Can't not find book review for book {@BookId} Create new Book review", request.BookId);
            ThrowHelper.ThrowNotFoundWhenItemIsNull(bookReviewViewModel,"không tìm thấy sách" );
        }

        var commentExitsByBookCombieUserRating = await _validationCommentServices.AnyAsync(c =>
            c.BookReviewId == bookReviewViewModel.Id && c.ReviewerId == _identityProvider.UserIdentity(), cancellationToken);
        ThrowHelper.ThrowBadRequestWhenITemIsNotNull(commentExitsByBookCombieUserRating, "Cảm ơn bạn nhưng bạn đã đánh giá tác phẩm này rồi");
        
        var comment = Core.BoundContext.BookReviewContext.CommentAggregate.Comment.Create(
            bookReviewId: bookReviewViewModel.Id,
            reviewerId: _identityProvider.UserIdentity(),
            content: request.Content,
            parentCommentId: request.ParentCommentId
        );
        _commentRepository.CreateComment(comment);
        _logger.LogInformation("Created comment for book is {@Id}",  request.BookId);
        await _commentRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return comment.MapToViewModel();
    }
}
