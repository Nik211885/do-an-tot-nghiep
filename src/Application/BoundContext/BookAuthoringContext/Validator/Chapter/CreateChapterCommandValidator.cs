using Application.BoundContext.BookAuthoringContext.Command.Chapter;
using Application.BoundContext.BookAuthoringContext.Message;
using FluentValidation;

namespace Application.BoundContext.BookAuthoringContext.Validator.Chapter;

public class CreateChapterCommandValidator : AbstractValidator<CreateChapterCommand>
{
    public CreateChapterCommandValidator()
    {
        RuleFor(c=>c.Content)
            .NotEmpty().WithMessage(ChapterValidationMessage.ContentRequired);
        RuleFor(c => c.Title)
            .NotEmpty().WithMessage(ChapterValidationMessage.TitleRequired)
            .MaximumLength(LengthPropForChapter.TitleMaxLength)
            .WithMessage(ChapterValidationMessage.TitleMaxLenght);
    }
}
