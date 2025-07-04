using Application.BoundContext.BookAuthoringContext.Command.Chapter;
using Application.BoundContext.BookAuthoringContext.Message;
using FluentValidation;

namespace Application.BoundContext.BookAuthoringContext.Validator.Chapter;

public class RenameChapterVersionValidator : AbstractValidator<RenameChapterVersionCommand>
{
    public RenameChapterVersionValidator()
    {
        RuleFor(c => c.NameVersion)
            .NotNull().WithMessage(ChapterValidationMessage.NameVersionRequired)
            .MaximumLength(LengthPropForChapter.MaxNameVersionChapter)
            .WithMessage(ChapterValidationMessage.NameVersionMaxLenght);
    }
}
