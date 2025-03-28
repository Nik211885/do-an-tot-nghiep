using Core.Entities.TestAggregate;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repository;

public class TestCaseRepository(IUnitOfWork unitOfWork, 
    ApplicationDbContext dbContext) : ITestCaseRepository
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;
    private readonly ApplicationDbContext _dbContext = dbContext;

    public TestCaseAggregate CreateTestCase(TestCaseAggregate testCaseAggregate)
    {
        _dbContext.ReadOnly().TestCases.Add(testCaseAggregate);
        return testCaseAggregate;
    }

    public TestCaseAggregate UpdateTestCase(TestCaseAggregate testCaseAggregate)
    {
        _dbContext.TestCases.Update(testCaseAggregate);
        return testCaseAggregate;
    }

    public void DeleteTestCase(TestCaseAggregate testCaseAggregate)
    {
        _dbContext.TestCases.Remove(testCaseAggregate);
    }

    public async Task<IReadOnlyCollection<TestCaseAggregate>> GetAllTestCasesAsync()
    {
       var result = await _dbContext.ReadOnly().TestCases.AsNoTracking().ToListAsync();
       return result.AsReadOnly();
    }
}
