using System.Text.Json.Serialization;

namespace Application.Models.KeyCloak;

public record UserInfo(
    [property: JsonPropertyName("id")]
    string Id,
    [property: JsonPropertyName("userName")]
    string UserName,
    [property: JsonPropertyName("firstName")]
    string FirstName,
    [property: JsonPropertyName("lastName")]
    string LastName,
    [property: JsonPropertyName("email")]
    string Email,
    [property: JsonPropertyName("emailVerified")]
    bool EmailVerified
);
