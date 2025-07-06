using System.Security.Claims;
using System.Text.Json;
using Application.Exceptions;
using Application.Interfaces.IdentityProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Keycloak;

public class IdentityProvider(IHttpContextAccessor contextAccessor)
    : IIdentityProvider
{
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Guid UserIdentity()
    {
        if (Guid.TryParse(_contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                out var value))
        {
            return value;
        }
        // you cant throw authorization when not find id in access token
        return Guid.Parse("00000000-0000-0000-0000-000000000000");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string UserName()
        => _contextAccessor.HttpContext?.User?.Claims.
            FirstOrDefault(c => c.Type == "name")?.Value ?? "Unknown";

    public bool IsInRole(string roleMatch)
    {
        var roleString = _contextAccessor.HttpContext?
            .User?.Claims.FirstOrDefault(x => x.Type == "realm_access")?.Value;
        if (roleString != null)
        {
            using var jsonDocument = JsonDocument.Parse(roleString);
            var roles = jsonDocument.RootElement.GetProperty("roles");
            foreach (var role in roles.EnumerateArray())
            {
                if (role.GetString() == roleMatch)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public IEnumerable<Claim> Claims()
    {
        return _contextAccessor.HttpContext.User.Claims;
    }

    public string FullName()
    {
        var firstName = _contextAccessor.HttpContext?.User?.FindFirst("given_name")?.Value;
        var lastName = _contextAccessor.HttpContext?.User?.FindFirst("family_name")?.Value;

        if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
            return $"{firstName} {lastName}";
        
        var fullName = _contextAccessor.HttpContext?.User?.FindFirst("name")?.Value;
        return fullName ?? "Unknown";
    }

    public bool IsAuthenticated()
    {
        var identity = _contextAccessor.HttpContext.User.Identity;
        return identity is not null && identity.IsAuthenticated;
    }
}
