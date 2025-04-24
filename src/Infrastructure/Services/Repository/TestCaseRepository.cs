using Core.Entities.TestAggregate;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repository;

// Some time I want to use sql raw make query complex or in ef expression compile
// it restrict with sql because it supports many dbms or use large case, so I have added Dapper
// in solution make query 

public class TestCaseRepository(ApplicationDbContext dbContext) 
    : Repository<TestCaseAggregate>(dbContext), ITestCaseRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    
    public async Task<IReadOnlyCollection<TestCaseAggregate>?> GetAllTestCasesAsync()
    { 
       var result = await _dbContext.TestCaseAggregate.AsNoTracking().ToListAsync();
       return result;
    }
}
