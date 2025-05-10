using System.Text.Json.Serialization;

namespace Core.BoundContext.WriteBookContext.ChapterAggregate;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ChapterStatus
{
    Draft = 1,
    Submitted = 2,
}
