using Core.Entities.TestAggregate;

namespace Core.Events;

public class CreatedTestCase(Guid testCaseId, TestLevel testLevel) : IEvent
{
    public Guid TestCaseId { get; } = testCaseId;
    public TestLevel TestLevel { get; } = testLevel;
}
