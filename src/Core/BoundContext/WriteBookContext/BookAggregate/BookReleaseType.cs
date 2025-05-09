using System.Text.Json.Serialization;

namespace Core.BoundContext.WriteBookContext.BookAggregate;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BookReleaseType
{
    Serialized = 1,
    Complete = 2
}
