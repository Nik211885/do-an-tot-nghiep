using System.Text.Json.Serialization;

namespace Core.BoundContext.BookManagement.BookAggregate;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BookStatus
{
    
}
