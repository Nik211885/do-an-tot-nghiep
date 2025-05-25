using Application.BoundContext.BookAuthoringContext.Command.Genre;
using Application.BoundContext.BookAuthoringContext.Message;
using Core.Interfaces.Repositories.BookAuthoringContext;
using FluentValidation;

namespace Application.BoundContext.BookAuthoringContext.Validator.Genre;

public class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
{
    // Fact if in validator has contains many validation with inf
    // you need separate from repository you need create query filter interface
    // or query extension
    public CreateGenreCommandValidator(IBookAuthoringQueryValidator queryValidator)
    {
        RuleFor(g => g.Name)
            .NotEmpty().WithMessage(GenreValidationMessage.NameRequired)
            .MaximumLength(100).WithMessage(GenreValidationMessage.NameMaxLength)
            .MustAsync(async(name, cancellationToken)=>await queryValidator.GenreBeUniqueNameAsync(name, cancellationToken))
            .WithMessage(GenreValidationMessage.NameAlreadyExists);
        RuleFor(g => g.Description)
            .NotEmpty().WithMessage(GenreValidationMessage.DescriptionRequired)
            .MaximumLength(500).WithMessage(GenreValidationMessage.DescriptionMaxLength);
        RuleFor(g => g.AvatarUrl)
            .NotEmpty().WithMessage(GenreValidationMessage.AvatarUrlRequired)
            .MaximumLength(200).WithMessage(GenreValidationMessage.AvatarUrlMaxLength);
    }
}
