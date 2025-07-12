using System.Text.Json.Serialization;

namespace Core.BoundContext.ModerationContext.BookApprovalAggregate;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ChapterApprovalStatus
{
    Pending,
    Rejected,
    Approved,
}
