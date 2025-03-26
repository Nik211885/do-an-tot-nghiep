namespace Core.Events;

public class CreatedTestEvent : AbsDomainEvent
{
    public string Name { get; set; } = string.Empty;
}
