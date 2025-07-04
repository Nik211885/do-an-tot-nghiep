using Application.BoundContext.BookReviewContext.Command.Comment;
using Application.BoundContext.BookReviewContext.Message;
using FluentValidation;

namespace Application.BoundContext.BookReviewContext.Validator.Comment;

internal class CreateCommentValidation : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentValidation()
    {
        RuleFor(c => c.Content)
            .MaximumLength(200)
            .WithMessage(CommentValidationMessage.ContentMaxLength)
            .NotEmpty()
            .WithMessage(CommentValidationMessage.ContentCanNotNull);
    }
}
