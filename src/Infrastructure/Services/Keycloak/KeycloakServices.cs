using System.Net.Http.Json;
using Application.Interfaces.IdentityProvider;
using Application.Models.KeyCloak;
using Infrastructure.Configurations;
using Infrastructure.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Keycloak;

public class KeycloakServices(IOptions<KeycloakConfiguration> keyCloakConfiguration,
    IHttpClientFactory clientFactory,
    ILogger<KeycloakServices> logger) : IIdentityProviderServices
{
    private readonly KeycloakConfiguration _keyCloakConfiguration = keyCloakConfiguration.Value;
    private readonly IHttpClientFactory _clientFactory = clientFactory;
    private readonly ILogger<KeycloakServices> _logger = logger;
    public async Task<TokenResult> GetTokenAsync()
    {
        var request = new Dictionary<string, string>()
        {
            { "grant_type", "client_credentials" },
            { "client_id", _keyCloakConfiguration.ClientId },
            { "client_secret", _keyCloakConfiguration.ClientSecret },
        };
        var requestFromUrlencodedContent = new FormUrlEncodedContent(request);
        try
        {
            var response = await GetHttpClient().PostAsync(string.Format(KeycloakApiUri.Token,_keyCloakConfiguration.Realm), requestFromUrlencodedContent);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error getting token in keycloak: {response}", response.ReasonPhrase);
                throw new Exception($"Failed to get token in keycloak: {response.ReasonPhrase}");
            }
            var token = await response.Content.ReadFromJsonAsync<TokenResult>();
            if (token is not null)
            {
                return token;
            }
            _logger.LogError("Response don't have property access_token in response: {response}", response);
            throw new Exception($"Response don't have property access_token in response:{response}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw new Exception($"An error occurred while fetching token: {ex.Message}", ex);
        }
    }

    public async Task<UserInfo?> GetUserInfoAsync(string id)
    {
        var response = await GetHttpClient().GetAsync(string.Format(KeycloakApiUri.GetUserInfo,_keyCloakConfiguration.Realm, id));
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<UserInfo>() : null;
    }
    private HttpClient GetHttpClient()
        => _clientFactory.CreateClient(HttpClientKeyFactory.KeyCloak);
}
