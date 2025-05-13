using System.Text.Json.Serialization;

namespace Core.BoundContext.BookAuthoringContext.BookAggregate;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BookReleaseType
{
    Serialized = 1,
    Complete = 2
}
