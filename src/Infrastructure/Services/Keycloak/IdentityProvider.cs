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
    public string UserIdentity() 
        => _contextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? "Unknown";
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string UserName()
        => _contextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "Unknown";
}
