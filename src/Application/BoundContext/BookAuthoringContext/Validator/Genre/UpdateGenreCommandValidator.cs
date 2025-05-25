using Application.BoundContext.BookAuthoringContext.Command.Genre;
using Application.BoundContext.BookAuthoringContext.Message;
using FluentValidation;

namespace Application.BoundContext.BookAuthoringContext.Validator.Genre;

public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
{
    public UpdateGenreCommandValidator(IBookAuthoringQueryValidator queryValidator)
    {
        RuleFor(g=>g.Request.Name)
            .NotEmpty().WithMessage(GenreValidationMessage.NameRequired)
            .MaximumLength(100).WithMessage(GenreValidationMessage.NameMaxLength)
            .MustAsync(async (command,name,cancellationToken)=>await queryValidator.GenreBeUniqueNameAsync(name, cancellationToken, command.Id))
            .WithMessage(GenreValidationMessage.NameAlreadyExists);
        RuleFor(g => g.Request.Description)
            .NotEmpty().WithMessage(GenreValidationMessage.DescriptionRequired)
            .MaximumLength(500).WithMessage(GenreValidationMessage.DescriptionMaxLength);
        RuleFor(g => g.Request.AvatarUrl)
            .NotEmpty().WithMessage(GenreValidationMessage.AvatarUrlRequired)
            .MaximumLength(200).WithMessage(GenreValidationMessage.AvatarUrlMaxLength);
    }
}
