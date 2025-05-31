using System.Net;
using Application.Interfaces.Cache;
using Application.Interfaces.IdentityProvider;
using Infrastructure.Options;
using Infrastructure.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Keycloak;

public class HttpClientKeyCloakInterceptor(IIdentityProviderServices identityProviderServices,
    ILogger<HttpClientKeyCloakInterceptor> logger,
    IOptions<KeycloakOptions> keycloakConfiguration,
    ICache cache) 
    : DelegatingHandler
{
    private readonly ICache _cache = cache;
    private readonly ILogger<HttpClientKeyCloakInterceptor> _logger = logger;
    private readonly IIdentityProviderServices _identityProviderServices = identityProviderServices;
    private readonly IOptions<KeycloakOptions> _keycloakConfiguration = keycloakConfiguration;
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content?.Headers.ContentType is { MediaType: "application/x-www-form-urlencoded" })
        {
            var responseWithClientSecret = await base.SendAsync(request, cancellationToken);
            if (responseWithClientSecret.IsSuccessStatusCode)
            {
                return responseWithClientSecret;
            }
            _logger.LogError("Failed to send client secret with application/x-www-form-urlencoded request: {request}, response: {response}", request, responseWithClientSecret);
            throw new Exception($"Failed to send client secret with application/x-www-form-urlencoded request: {request}, response: {responseWithClientSecret}");
        }
        var tokenKey = $"Keycloak:AccessToken{_keycloakConfiguration.Value.ClientId}";
        var token = await _cache.GetAsync(tokenKey);
        // First if token is empty send request to idp get token
        if (string.IsNullOrEmpty(token))
        {
            var tokenResult = await _identityProviderServices.GetTokenAsync();
            token = tokenResult.AccessToken;
            await _cache.SetAsync(tokenKey, tokenResult.AccessToken,tokenResult.ExpiresIn);
        }
        request.AddBearerToken(token);
        // Else token not empty just add token to request and send back to idp
            // If token has express i just call back to idp and get token after send back idp
        var response = await base.SendAsync(request, cancellationToken);
        if ((int)response.StatusCode > 500)
        {
            _logger.LogError("Error while sending keycloak request: {request}, response {response}", request,
                response.ReasonPhrase);
            throw new Exception($"Error while sending keycloak response {response.ReasonPhrase}");
        }
        // End session provider new token
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var tokenResult = await _identityProviderServices.GetTokenAsync();
            token = tokenResult.AccessToken;
            await _cache.SetAsync(tokenKey, token, tokenResult.ExpiresIn);
            request.AddBearerToken(token);
            var retryResponse = await base.SendAsync(request, cancellationToken);
            if (retryResponse.StatusCode != HttpStatusCode.Unauthorized && (int)retryResponse.StatusCode < 500)
            {
                return retryResponse;
            }
            _logger.LogError("Token has express with key cloak idp very sort {request}, response {response}", request, response.ReasonPhrase);
            throw new Exception($"Token has express with key cloak idp very sort {response.ReasonPhrase}");
        }

        return response;
    }
}
