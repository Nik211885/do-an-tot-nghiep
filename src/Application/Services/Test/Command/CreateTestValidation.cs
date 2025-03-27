using FluentValidation;

namespace Application.Services.Test.Command;

public class CreateTestValidation : AbstractValidator<CreateTestCommand>
{
    public CreateTestValidation()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
    }
}
