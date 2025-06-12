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
    public DateTimeOffset SearchDate { get; private set; }
    protected SearchHistory(){}

    private SearchHistory(Guid userId, string searchTerm)
    {
        UserId = userId;
        SearchTerm = searchTerm;
    }

    public static SearchHistory Create(Guid userId, string searchTerm)
    {
        return new SearchHistory(userId, searchTerm);
    }

    public void Delete()
    {
        // Mark make delete
    }
}
