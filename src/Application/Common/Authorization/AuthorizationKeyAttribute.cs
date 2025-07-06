using Application.Helper;
using Microsoft.AspNetCore.Authorization;

namespace Application.Common.Authorization;

public class AuthorizationKeyAttribute
    : Attribute, IAuthorizationRequirement, IAuthorizationRequirementData
{
    public string? Roles { get; set; }
    public AuthorizationKeyAttribute()
    {
        
    }
    public AuthorizationKeyAttribute(Role role)
    {
        Roles = role.GetDescriptionAttribute();
    }
    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }
}
