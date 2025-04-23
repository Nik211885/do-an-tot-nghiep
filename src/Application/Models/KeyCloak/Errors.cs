using System.Text.Json.Serialization;

namespace Application.Models.KeyCloak;

public record Errors(
    [property: JsonPropertyName("error")] 
    string Error
);
