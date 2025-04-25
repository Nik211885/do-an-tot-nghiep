using FluentValidation;

namespace Application.Services.Test.Command.Created;

public class CreateTestValidation : AbstractValidator<CreateTestCommand>
{
    public CreateTestValidation()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
    }
}
