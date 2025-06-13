using Application.Models.EventBus;
using Core.Events;

public class BookSearchedIntegrationEvent(Guid userId, string stringSearch, string ipAddress)
    : IntegrationEvent{
    public Guid UserId { get; } = userId;
    public string StringSearch { get; } = stringSearch;
    public string IpAddress { get; } = ipAddress;   
}
