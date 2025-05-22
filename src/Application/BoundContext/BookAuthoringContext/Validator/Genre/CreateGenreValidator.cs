using Application.BoundContext.BookAuthoringContext.Command.Genre;
using Application.BoundContext.BookAuthoringContext.Message;
using Core.Interfaces.Repositories.BookAuthoringContext;
using FluentValidation;

namespace Application.BoundContext.BookAuthoringContext.Validator.Genre;

public class CreateGenreValidator : AbstractValidator<CreateGenreCommand>
{
    // Fact if in validator has contains many validation with inf
    // you need separate from repository you need create query filter interface
    // or query extension
    public CreateGenreValidator(IBookAuthoringQueryValidator queryValidator)
    {
        RuleFor(g => g.Name)
            .NotEmpty().WithMessage(GenreMessage.NameRequired)
            .MaximumLength(100).WithMessage(GenreMessage.NameMaxLength)
            .MustAsync(async(name, cancellationToken)=>await queryValidator.GenreBeUniqueName(name, cancellationToken))
            .WithMessage(GenreMessage.NameAlreadyExists);
        RuleFor(g => g.Description)
            .NotEmpty().WithMessage(GenreMessage.DescriptionRequired)
            .MaximumLength(500).WithMessage(GenreMessage.DescriptionMaxLength);
        RuleFor(g => g.AvatarUrl)
            .NotEmpty().WithMessage(GenreMessage.AvatarUrlRequired)
            .MaximumLength(200).WithMessage(GenreMessage.AvatarUrlMaxLength);
    }
}
