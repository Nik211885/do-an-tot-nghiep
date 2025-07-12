using Application.Helper;
using Microsoft.AspNetCore.Authorization;

namespace Application.Common.Authorization;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
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
