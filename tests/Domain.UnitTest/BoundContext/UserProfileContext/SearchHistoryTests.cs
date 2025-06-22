using Core.BoundContext.UserProfileContext.SearchHistoryAggregate;

namespace Domain.UnitTest.BoundContext.UserProfileContext;

public class SearchHistoryTests
{
    [Fact]
    public void Create_Should_Succeed_With_Valid_Data()
    {
        var userId = Guid.NewGuid();
        var term = "search";
        var history = SearchHistory.Create(userId, term);
        Assert.Equal(userId, history.UserId);
        Assert.Equal(term, history.SearchTerm);
    }
}
