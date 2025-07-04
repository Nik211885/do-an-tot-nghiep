using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
    
    public static StringContent GetStringContent(this object request)
    {
        var options = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    
        var json = JsonSerializer.Serialize(request, options);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}
