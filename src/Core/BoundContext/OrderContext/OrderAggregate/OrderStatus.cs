using System.Text.Json.Serialization;

namespace Core.BoundContext.OrderContext.OrderAggregate;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    /// <summary>
    /// 
    /// </summary>
    Pending = 0,
    /// <summary>
    /// 
    /// </summary>
    Success = 1,
    /// <summary>
    /// 
    /// </summary>
    Failure = 2,
    /// <summary>
    /// 
    /// </summary>
    Canceled = 3
}
