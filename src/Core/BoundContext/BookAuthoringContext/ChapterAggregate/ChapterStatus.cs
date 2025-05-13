using System.Text.Json.Serialization;

namespace Core.BoundContext.BookAuthoringContext.ChapterAggregate;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ChapterStatus
{
    Draft = 1,
    Submitted = 2,
}
