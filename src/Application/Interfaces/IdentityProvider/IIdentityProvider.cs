using System.Security.Claims;

namespace Application.Interfaces.IdentityProvider;
/// <summary>
///     Get claim in to access token
/// </summary>
public interface IIdentityProvider
{
    /// <summary>
    ///   Get user id in token if you don't have token return "unknown" ????
    /// </summary>
    Guid UserIdentity();

    /// <summary>
    ///     Get username in token if you don't have token return "unknown" ??
    /// </summary>
    string UserName();
    /*bool IsAuthenticated { get; }*/
    /// <summary>
    ///  Check user has role is role name
    /// </summary>
    /// <param name="role">role of action for endpoint</param>
    /// <returns>
    ///     Return true if user has role for endpoint
    /// </returns>
    bool IsInRole(string role);
    /*/// <summary>
    ///     Check user has permission should endpoint need pass authorize 
    /// </summary>
    /// <param name="permission">Permission for access rehouse</param>
    /// <returns>
    ///     Return true if use has permission
    /// </returns>*/
    /*Task<bool> IsInPermissionAsync(string permission);*/
    IEnumerable<Claim> Claims();
    string FullName();
    bool IsAuthenticated();
}
