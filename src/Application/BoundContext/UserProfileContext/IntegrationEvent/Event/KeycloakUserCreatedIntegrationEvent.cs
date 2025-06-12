using System.Text.Json.Serialization;

namespace Application.BoundContext.UserProfileContext.IntegrationEvent.Event;

public class KeycloakUserCreatedIntegrationEvent :
    Models.EventBus.IntegrationEvent
{
    [JsonPropertyName("@class")]
    public string Class { get; set; }
    [JsonPropertyName("time")]
    public long Time { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("realmId")]
    public string RealmId { get; set; }
    [JsonPropertyName("clientId")]
    public string ClientId { get; set; }
    [JsonPropertyName("userId")]
    public string UserId { get; set; }
    [JsonPropertyName("ipAddress")]
    public string IpAddress { get; set; }
    [JsonPropertyName("details")]
    public UserCreatedDetail Detail { get; set; }
}

public class UserCreatedDetail
{
    [JsonPropertyName("identity_provider")]
    public string IdentityProvider { get; set; }
    [JsonPropertyName("register_method")]
    public string RegisterMethod { get; set; }
    [JsonPropertyName("identity_provider_identity")]
    public string IdentityProviderIdentity { get; set; }
    [JsonPropertyName("code_id")]
    public string  CodeId { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("username")]
    public string UserName { get; set; }
}
