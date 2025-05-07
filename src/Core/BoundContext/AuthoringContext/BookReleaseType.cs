using System.Text.Json.Serialization;

namespace Core.BoundContext.BookManagement.BookAggregate;
/// <summary>
/// 
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BookReleaseType
{
    /// <summary>
    /// 
    /// </summary>
    Serialized = 1,
    /// <summary>
    /// 
    /// </summary>
    Complete = 2
}
