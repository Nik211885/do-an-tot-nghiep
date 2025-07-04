using Core.Entities;
using Core.Interfaces;

namespace Core.BoundContext.UserProfileContext.SearchHistoryAggregate;

public class SearchHistory : BaseEntity, IAggregateRoot
{
    public string SearchTerm { get; private set; }
    public Guid UserId { get; private set; }
    // You can add more prop like ip for user 
    // rank serch and so more but i just demo
    //
    public string IpAddress { get; private set; }
    public int SearchCout { get; private set; }
    public DateTimeOffset SearchDate { get; private set; }
    protected SearchHistory(){}

    private SearchHistory(Guid userId, string searchTerm, string ipAddress, int searchCout)
    {
        UserId = userId;
        SearchCout = searchCout;
        IpAddress = ipAddress;
        SearchTerm = searchTerm;
        SearchDate = DateTimeOffset.UtcNow;
    }

    public static SearchHistory Create(Guid userId, string searchTerm,string ipAddress, int searchCout)
    {
        return new SearchHistory(userId, searchTerm, ipAddress, searchCout);
    }

    public void Delete()
    {
        // Mark make delete
    }
}
