using Application.BoundContext.BookReviewContext.Command.Comment;
using Application.BoundContext.BookReviewContext.Message;
using FluentValidation;

namespace Application.BoundContext.BookReviewContext.Validator.Comment;

public class UpdateCommentValidation : AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentValidation()
    {
        RuleFor(c => c.Content)
            .MaximumLength(200)
            .WithMessage(CommentValidationMessage.ContentMaxLength)
            .NotEmpty()
            .WithMessage(CommentValidationMessage.ContentCanNotNull);
    }    
}
