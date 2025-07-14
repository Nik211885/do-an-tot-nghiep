using System.Text.Json.Serialization;

namespace Core.BoundContext.BookAuthoringContext.ChapterAggregate;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ChapterStatus
{
    Draft = 1,
    Submitted = 2,
    /*Pending = 3,*/
    Rejected = 4,
    Approved = 5,
}
