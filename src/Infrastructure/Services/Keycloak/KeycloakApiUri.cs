namespace Infrastructure.Services.Keycloak;

public static class KeycloakApiUri
{
    /// <summary>
    ///  First argument is my realms
    /// </summary>
    public const string Token = "realms/{0}/protocol/openid-connect/token";
    /// <summary>
    ///  First argument is my realms and second argument is user id want to get information
    /// </summary>
    public const string GetUserInfo = "admin/realms/{0}/users/{1}";

    public const string UpdateUserInfo = "admin/realms/{0}/users/{1}";
    public const string ResetPassword = "/admin/realms/{0}/users/{1}/reset-password-email";
}
