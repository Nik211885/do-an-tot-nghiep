using System.Text.Json.Serialization;

namespace Application.Models.KeyCloak;

public record TokenResult(
    [property: JsonPropertyName("access_token")]
    string AccessToken,
    [property: JsonPropertyName("expires_in")]
    int ExpiresIn
);
