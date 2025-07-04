using Application.BoundContext.BookReviewContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Core.Interfaces.Repositories.BookReviewContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookReviewContext.Command.Comment;

public record UpdateCommentCommand(Guid Id, string Content) : 
    IBookReviewCommand<CommentViewModel>;

public class UpdateCommentCommandHandler(
    ILogger<UpdateCommentCommandHandler> logger,
    ICommentRepository commentRepository)
    : ICommandHandler<UpdateCommentCommand, CommentViewModel>
{
    private readonly ILogger<UpdateCommentCommandHandler> _logger = logger;
    private readonly ICommentRepository _commentRepository = commentRepository;
    public async Task<CommentViewModel> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(request.Id, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(comment, "Bình luận không tìm thấy");
        comment.UpdateComment(request.Content);
        _logger.LogInformation("Update comment has id {@Id}", request.Id);
        _commentRepository.UpdateComment(comment);
        await _commentRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return comment.MapToViewModel();
    }
}
