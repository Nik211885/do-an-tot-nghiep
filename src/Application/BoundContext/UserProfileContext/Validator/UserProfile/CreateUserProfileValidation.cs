using Application.BoundContext.UserProfileContext.Command.UserProfile;
using Application.BoundContext.UserProfileContext.Message;
using Application.Interfaces.Validator;
using Elastic.Clients.Elasticsearch;
using FluentValidation;
using Org.BouncyCastle.Bcpg;

namespace Application.BoundContext.UserProfileContext.Validator.UserProfile;

public class CreateUserProfileValidation
    : AbstractValidator<CreateUserProfileCommand>
{
    public CreateUserProfileValidation(IValidationServices<Core.BoundContext.UserProfileContext.UserProfileAggregate.UserProfile> userProfilesValidationServices)
    {
        RuleFor(x => x.Bio)
            .MaximumLength(500)
            .WithMessage(UserProfileValidationMessage.MaxBioLength);
        RuleFor(x => x.UserId)
            .MustAsync(async (userId, cancellationToken) => await userProfilesValidationServices.AnyAsync(u=>u.Id == userId, cancellationToken) is null)
            .WithMessage(UserProfileValidationMessage.HasSyncUserFromKeyCloak);
    }
}
