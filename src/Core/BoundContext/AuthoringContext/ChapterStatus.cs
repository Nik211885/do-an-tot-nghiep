using System.Text.Json.Serialization;

namespace Core.BoundContext.BookManagement.BookAggregate;
/// <summary>
/// 
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ChapterStatus
{
    /// <summary>
    /// 
    /// </summary>
    Draft = 1,
    /// <summary>
    /// 
    /// </summary>
    ///
    Submitted = 2,
}
