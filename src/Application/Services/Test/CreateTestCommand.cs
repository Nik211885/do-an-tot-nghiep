using Application.Interfaces.CQRS;
using FluentValidation;

namespace Application.Services.Test;

public record CreateTestCommand(string Name, string Description) : ICommand<Guid>;

public class CreateTestCommandValidator : AbstractValidator<CreateTestCommand>
{
    public CreateTestCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithName("Name test case is required");
        RuleFor(x => x.Description).NotEmpty().WithName("Description test case is required");
    }
}
