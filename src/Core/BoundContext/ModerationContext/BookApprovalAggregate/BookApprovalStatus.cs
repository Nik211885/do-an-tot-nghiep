using System.Text.Json.Serialization;

namespace Core.BoundContext.ModerationContext.BookApprovalAggregate;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BookApprovalStatus
{
    Pending,
    Rejected,
    Approved,
}
