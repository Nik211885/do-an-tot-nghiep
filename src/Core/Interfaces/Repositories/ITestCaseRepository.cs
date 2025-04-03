using Core.Entities.TestAggregate;

namespace Core.Interfaces.Repositories;

public interface ITestCaseRepository : IRepository<TestCaseAggregate>
{
    Task<IReadOnlyCollection<TestCaseAggregate>> GetAllTestCasesAsync();
}
