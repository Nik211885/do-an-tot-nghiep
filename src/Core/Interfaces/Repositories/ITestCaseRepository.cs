using Core.Entities.TestAggregate;
using Core.Models;

namespace Core.Interfaces.Repositories;

public interface ITestCaseRepository : IRepository<TestCaseAggregate>
{
    Task<IReadOnlyCollection<TestCaseAggregate>?> GetAllTestCasesAsync();
    Task<PaginationItem<TestCaseAggregate>> GetTestCaseWithPaginationAsync(int pageNumber, int pageSize);
}
