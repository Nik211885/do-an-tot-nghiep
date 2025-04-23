using System.Text.Json.Serialization;
using Application.Models.KeyCloak;

namespace Application.Interfaces.IdentityProvider;

public interface IIdentityProviderServices
{
    Task<TokenResult> GetTokenAsync();
    Task<UserInfo?> GetUserInfoAsync(string id);
}




