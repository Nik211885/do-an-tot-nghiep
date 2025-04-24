using System.Net.Http.Headers;

namespace Infrastructure.Helper;
/// <summary>
///     Some helper to httpclient 
/// </summary>
public static class HttpClientHelper
{
    /// <summary>
    ///     Add Bearer Authorization to request
    /// </summary>
    /// <param name="request">HttpRequest</param>
    /// <param name="token">Token Bearer</param>
    public static void AddBearerToken(this HttpRequestMessage request, string token)
        => request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
}
