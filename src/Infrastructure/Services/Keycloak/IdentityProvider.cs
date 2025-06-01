using System.Security.Claims;
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
}
