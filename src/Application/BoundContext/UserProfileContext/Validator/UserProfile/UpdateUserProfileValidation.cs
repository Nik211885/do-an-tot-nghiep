using Application.BoundContext.UserProfileContext.Command.UserProfile;
using Application.BoundContext.UserProfileContext.Message;
using FluentValidation;

namespace Application.BoundContext.UserProfileContext.Validator.UserProfile;

public class UpdateUserProfileValidation : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileValidation()
    {
        RuleFor(x => x.Update.Bio)
            .MaximumLength(500)
            .WithMessage(UserProfileValidationMessage.MaxBioLength);
    }
}
