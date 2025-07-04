using Application.BoundContext.BookAuthoringContext.Command.Chapter;
using Application.BoundContext.BookAuthoringContext.Message;
using Application.BoundContext.BookAuthoringContext.Validator.Book;
using FluentValidation;

namespace Application.BoundContext.BookAuthoringContext.Validator.Chapter;

public class UpdateChapterCommandValidator : AbstractValidator<UpdateChapterCommand>
{
    public UpdateChapterCommandValidator()
    {
        RuleFor(x => x.Request.Content)
            .NotEmpty().WithMessage(ChapterValidationMessage.ContentRequired);
        RuleFor(x => x.Request.Title)
            .NotEmpty().WithMessage(ChapterValidationMessage.TitleRequired)
            .MaximumLength(LengthPropForChapter.TitleMaxLength)
            .WithMessage(ChapterValidationMessage.TitleMaxLenght);
    }
}
