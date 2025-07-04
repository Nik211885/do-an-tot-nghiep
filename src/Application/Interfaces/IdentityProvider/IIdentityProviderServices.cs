using System.Text.Json.Serialization;
using Application.BoundContext.UserProfileContext.Command.UserProfile;
using Application.Models.KeyCloak;

namespace Application.Interfaces.IdentityProvider;

public interface IIdentityProviderServices
{
    Task<TokenResult> GetTokenAsync();
    Task<UserInfo?> GetUserInfoAsync(string id);
    Task<bool> UpdateUserInfoAsync(UpdateUserProfileCommand update);
    Task<bool> ResetPasswordAsync(Guid userId, string clientId, string returnUrl);
}




