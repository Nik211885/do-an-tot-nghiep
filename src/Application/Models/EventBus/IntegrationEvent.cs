using System.Text.Json.Serialization;

namespace Application.Models.EventBus;

public abstract class IntegrationEvent
{
    [JsonInclude]
    public Guid Id { get; set; }
    [JsonInclude]
    public DateTimeOffset Timestamp { get; set; }

    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        Timestamp = DateTimeOffset.UtcNow;
    }
}
