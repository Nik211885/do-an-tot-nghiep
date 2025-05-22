using Application.BoundContext.BookAuthoringContext.Command.Book;
using Application.BoundContext.BookAuthoringContext.Message;
using FluentValidation;

namespace Application.BoundContext.BookAuthoringContext.Validator;

public class CreateBookValidator : AbstractValidator<CreateBookAuthoringCommand>
{
    public CreateBookValidator()
    {
        RuleFor(b => b.Title)
            .NotEmpty().WithMessage(BookMessage.TitleRequired)
            .MaximumLength(50).WithMessage(BookMessage.TitleMaxLength50);

        RuleFor(b => b.AvatarUrl)
            .MaximumLength(250).WithMessage(BookMessage.AvatarUrlMaxLength250)
            .When(b => !string.IsNullOrWhiteSpace(b.AvatarUrl));

        RuleFor(b => b.Description)
            .MaximumLength(500).WithMessage(BookMessage.DescriptionMaxLength500)
            .When(b => !string.IsNullOrWhiteSpace(b.Description));

        RuleFor(b => b.ReaderBookPolicy)
            .IsInEnum().WithMessage(BookMessage.ReaderBookPolicyInvalid);

        RuleFor(b => b.BookReleaseType)
            .IsInEnum().WithMessage(BookMessage.BookReleaseTypeInvalid);

        RuleForEach(b => b.TagsName)
            .NotEmpty().WithMessage(BookMessage.TagNameRequired)
            .MaximumLength(50).WithMessage(BookMessage.TagNameMaxLength50);

        RuleFor(b => b.GenreIds)
            .NotEmpty().WithMessage(BookMessage.GenreIdsRequired);

        RuleFor(b => b.VersionNumber)
            .GreaterThan(0).WithMessage(BookMessage.VersionNumberGreaterThanZero);
    }
}
