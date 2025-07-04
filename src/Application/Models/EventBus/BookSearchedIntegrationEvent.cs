namespace Application.Models.EventBus;

public class BookSearchedIntegrationEvent
    : IntegrationEvent{
    public Guid UserId { get; }
    public string StringSearch { get; }
    public string IpAddress { get; } 
    public int CoutResultForSearch { get; }

    public BookSearchedIntegrationEvent(Guid userId, string stringSearch, string ipAddress, int coutResultForSearch)
    {
        UserId = userId;
        StringSearch = stringSearch;
        IpAddress = ipAddress;
        CoutResultForSearch = coutResultForSearch;
    }
}
