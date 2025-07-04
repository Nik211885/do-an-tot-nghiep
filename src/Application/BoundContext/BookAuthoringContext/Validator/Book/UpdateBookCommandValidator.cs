using Application.BoundContext.BookAuthoringContext.Command.Book;
using Application.BoundContext.BookAuthoringContext.Message;
using FluentValidation;

namespace Application.BoundContext.BookAuthoringContext.Validator.Book;

public class UpdateBookCommandValidator :  AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
        RuleFor(b => b.Request.Title)
            .NotEmpty()
            .WithMessage(BookValidationMessages.TitleRequired)
            .MaximumLength(LengthPropForBook.TitleMaxLenght)
            .WithMessage(string.Format(BookValidationMessages.TitleMaxLength,LengthPropForBook.TitleMaxLenght));

        RuleFor(b => b.Request.AvatarUrl)
            .MaximumLength(LengthPropForBook.AvatarUrlMaxLenght)
            .WithMessage(string.Format(BookValidationMessages.AvatarUrlMaxLength,LengthPropForBook.AvatarUrlMaxLenght))
            .When(b => !string.IsNullOrWhiteSpace(b.Request.AvatarUrl));

        RuleFor(b => b.Request.Description)
            .MaximumLength(LengthPropForBook.DescriptionMaxLenght)
            .WithMessage(string.Format(BookValidationMessages.DescriptionCanNotNull, LengthPropForBook.DescriptionMaxLenght))
            .When(b => !string.IsNullOrWhiteSpace(b.Request.Description));

        RuleFor(b => b.Request.ReaderBookPolicy)
            .IsInEnum()
            .WithMessage(BookValidationMessages.ReaderBookPolicyInvalid);

        RuleFor(b => b.Request.BookReleaseType)
            .IsInEnum()
            .WithMessage(BookValidationMessages.BookReleaseTypeInvalid);

        RuleForEach(b => b.Request.TagsName)
            .NotEmpty()
            .WithMessage(BookValidationMessages.TagNameRequired)
            .MaximumLength(LengthPropForBook.TagNameMaxLenght)
            .WithMessage(string.Format(BookValidationMessages.TagNameMaxLength, LengthPropForBook.TagNameMaxLenght));

        RuleFor(b => b.Request.GenreIds)
            .NotEmpty()
            .WithMessage(BookValidationMessages.GenreIdsRequired);

        /*RuleFor(b => b.VersionNumber)
            .GreaterThan(0)
            .WithMessage(BookValidationMessages.VersionNumberGreaterThanZero);*/
    }
}
