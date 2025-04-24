namespace Application.Interfaces.IdentityProvider;
/// <summary>
///     Get claim in to access token
/// </summary>
public interface IIdentityProvider
{
    /// <summary>
    ///   Get user id in token if you don't have token return "unknown" ????
    /// </summary>
    string UserId();

    /// <summary>
    ///     Get username in token if you don't have token return "unknown" ??
    /// </summary>
    string UserName();
    /*bool IsAuthenticated { get; }*/
    
}
