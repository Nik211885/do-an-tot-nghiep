using System.Net.Http.Headers;

namespace Infrastructure.Helper;

public static class HttpClientHelper
{
    public static void AddBearerToken(this HttpRequestMessage request, string token)
        => request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
}
