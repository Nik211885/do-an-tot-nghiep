using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Domain.UnitTest.BoundContext.ModerationContext;

public class ApprovalDecisionTests
{
    [Fact]
    public void Create_Should_Succeed_With_Valid_Data()
    {
        var moderatorId = Guid.NewGuid();
        var note = "Ghi chú";
        var isAutomated = false;
        var status = BookApprovalStatus.Approved;
        var decision = ApprovalDecision.Create(moderatorId, note, isAutomated, status);
        Assert.Equal(moderatorId, decision.ModeratorId);
        Assert.Equal(note, decision.Note);
        Assert.Equal(isAutomated, decision.IsAutomated);
        Assert.Equal(status, decision.Status);
        Assert.True(decision.DecisionDateTime <= DateTimeOffset.UtcNow);
    }

    [Fact]
    public void Create_Should_Succeed_With_Null_Moderator_And_Note()
    {
        var decision = ApprovalDecision.Create(null, null, true, BookApprovalStatus.Rejected);
        Assert.Null(decision.ModeratorId);
        Assert.Null(decision.Note);
        Assert.True(decision.IsAutomated);
        Assert.Equal(BookApprovalStatus.Rejected, decision.Status);
    }
}
