using Core.Entities.TestAggregate;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repository;

public class TestCaseRepository(ApplicationDbContext dbContext) 
    : Repository<TestCaseAggregate>(dbContext), ITestCaseRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    public async Task<IReadOnlyCollection<TestCaseAggregate>> GetAllTestCasesAsync()
    {
       var result = await _dbContext.ReadOnly().TestCases.AsNoTracking().ToListAsync();
       return result.AsReadOnly();
    }
}
