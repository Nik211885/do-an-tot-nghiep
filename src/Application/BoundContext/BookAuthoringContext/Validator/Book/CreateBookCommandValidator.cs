using Application.BoundContext.BookAuthoringContext.Command.Book;
using Application.BoundContext.BookAuthoringContext.Message;
using FluentValidation;

namespace Application.BoundContext.BookAuthoringContext.Validator.Book;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(b => b.Title)
            .NotEmpty()
            .WithMessage(BookValidationMessages.TitleRequired)
            .MaximumLength(LengthPropForBook.TitleMaxLenght)
            .WithMessage(string.Format(BookValidationMessages.TitleMaxLength,LengthPropForBook.TitleMaxLenght));

        RuleFor(b => b.AvatarUrl)
            .MaximumLength(LengthPropForBook.AvatarUrlMaxLenght)
            .WithMessage(string.Format(BookValidationMessages.AvatarUrlMaxLength,LengthPropForBook.AvatarUrlMaxLenght))
            .When(b => !string.IsNullOrWhiteSpace(b.AvatarUrl));

        RuleFor(b => b.Description)
            .NotNull()
            .WithMessage(BookValidationMessages.DescriptionCanNotNull);

        RuleFor(b => b.ReaderBookPolicy)
            .IsInEnum()
            .WithMessage(BookValidationMessages.ReaderBookPolicyInvalid);

        RuleFor(b => b.BookReleaseType)
            .IsInEnum()
            .WithMessage(BookValidationMessages.BookReleaseTypeInvalid);

        RuleForEach(b => b.TagsName)
            .NotEmpty()
            .WithMessage(BookValidationMessages.TagNameRequired)
            .MaximumLength(LengthPropForBook.TagNameMaxLenght)
            .WithMessage(string.Format(BookValidationMessages.TagNameMaxLength, LengthPropForBook.TagNameMaxLenght));

        RuleFor(b => b.GenreIds)
            .NotEmpty()
            .WithMessage(BookValidationMessages.GenreIdsRequired);

        /*RuleFor(b => b.VersionNumber)
            .GreaterThan(0)
            .WithMessage(BookValidationMessages.VersionNumberGreaterThanZero);*/
    }
}
