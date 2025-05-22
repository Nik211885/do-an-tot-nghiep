using Application.BoundContext.BookAuthoringContext.Command.Genre;
using Application.BoundContext.BookAuthoringContext.Message;
using FluentValidation;

namespace Application.BoundContext.BookAuthoringContext.Validator.Genre;

public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
{
    public UpdateGenreCommandValidator(IBookAuthoringQueryValidator queryValidator)
    {
        RuleFor(g=>g.Request.Name)
            .NotEmpty().WithMessage(GenreMessage.NameRequired)
            .MaximumLength(100).WithMessage(GenreMessage.NameMaxLength)
            .MustAsync(async (command,name,cancellationToken)=>await queryValidator.GenreBeUniqueName(name, cancellationToken, command.Id))
            .WithMessage(GenreMessage.NameAlreadyExists);
        RuleFor(g => g.Request.Description)
            .NotEmpty().WithMessage(GenreMessage.DescriptionRequired)
            .MaximumLength(500).WithMessage(GenreMessage.DescriptionMaxLength);
        RuleFor(g => g.Request.AvatarUrl)
            .NotEmpty().WithMessage(GenreMessage.AvatarUrlRequired)
            .MaximumLength(200).WithMessage(GenreMessage.AvatarUrlMaxLength);
    }
}
