using Application.BoundContext.BookAuthoringContext.Command.Genre;
using Application.BoundContext.BookAuthoringContext.Message;
using Application.Interfaces.Validator;
using Core.BoundContext.BookAuthoringContext.GenresAggregate;
using FluentValidation;

namespace Application.BoundContext.BookAuthoringContext.Validator.Genre;

public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
{
    public UpdateGenreCommandValidator(IValidationServices<Genres> queryValidator)
    {
        RuleFor(g=>g.Request.Name)
            .NotEmpty().WithMessage(GenreValidationMessage.NameRequired)
            .MaximumLength(100).WithMessage(GenreValidationMessage.NameMaxLength)
            .MustAsync(async (command,name,cancellationToken)=>
                await queryValidator.AnyAsync(genre=> 
                        genre.Name == name && genre.Id != command.Id,
                    cancellationToken)  is null)
            .WithMessage(GenreValidationMessage.NameAlreadyExists);
        RuleFor(g => g.Request.Description)
            .NotEmpty().WithMessage(GenreValidationMessage.DescriptionRequired)
            .MaximumLength(500).WithMessage(GenreValidationMessage.DescriptionMaxLength);
        RuleFor(g => g.Request.AvatarUrl)
            .NotEmpty().WithMessage(GenreValidationMessage.AvatarUrlRequired)
            .MaximumLength(200).WithMessage(GenreValidationMessage.AvatarUrlMaxLength);
    }
}
