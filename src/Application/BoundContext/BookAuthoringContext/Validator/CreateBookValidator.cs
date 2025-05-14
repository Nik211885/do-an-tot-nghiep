using Application.BoundContext.BookAuthoringContext.Command;
using FluentValidation;

namespace Application.BoundContext.BookAuthoringContext.Validator;

public class CreateBookValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookValidator()
    {
        
    }
}
