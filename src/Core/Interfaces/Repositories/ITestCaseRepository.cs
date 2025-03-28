using Core.Entities.TestAggregate;

namespace Core.Interfaces.Repositories;

public interface ITestCaseRepository : IRepository<TestCaseAggregate>
{
    TestCaseAggregate CreateTestCase(TestCaseAggregate testCaseAggregate);
    TestCaseAggregate UpdateTestCase(TestCaseAggregate testCaseAggregate);
    void DeleteTestCase(TestCaseAggregate testCaseAggregate);
    Task<IReadOnlyCollection<TestCaseAggregate>> GetAllTestCasesAsync();
}
