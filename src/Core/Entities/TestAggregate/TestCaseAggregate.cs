using Core.Interfaces;

namespace Core.Entities.TestAggregate;

public class TestCaseAggregate : BaseEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public TestLevel TestLevel { get; private set; }
    
    public TestCaseAggregate(string name, TestLevel testLevel)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace("Name test case", name);
        Name = name;
        TestLevel = testLevel;
    }
}

public enum TestLevel
{
    Information,
    Warning,
    Error   
}
