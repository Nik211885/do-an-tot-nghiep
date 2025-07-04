using Core.BoundContext.UserProfileContext.SearchHistoryAggregate;

namespace Domain.UnitTest.BoundContext.UserProfileContext;

public class SearchHistoryTests
{
    [Fact]
    public void Create_Should_Succeed_With_Valid_Data()
    {
        var userId = Guid.NewGuid();
        var term = "search";
        var ip = "127.0.0.1";
        var cout = 1000;
        var history = SearchHistory.Create(userId, term, ip, cout);
        Assert.Equal(userId, history.UserId);
        Assert.Equal(term, history.SearchTerm);
    }
}
