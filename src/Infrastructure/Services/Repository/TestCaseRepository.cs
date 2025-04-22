using Core.Entities.TestAggregate;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Dapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repository;

// Some time I want to use sql raw make query complex or in ef expression compile
// it restrict with sql because it supports many dbms or use large case, so I have added Dapper
// in solution make query complex

public class TestCaseRepository(ApplicationDbContext dbContext) 
    : Repository<TestCaseAggregate>(dbContext), ITestCaseRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    
    public async Task<IReadOnlyCollection<TestCaseAggregate>?> GetAllTestCasesAsync()
    { 
        await using var dbContext = _dbContext.ReadOnly().Database.GetDbConnection();
       var sqlQuery = "SELECT * FROM TestCaseAggregate;";
       var result = await dbContext.QueryAsync<TestCaseAggregate>(sqlQuery);
       return result as  IReadOnlyCollection<TestCaseAggregate>;
    }
}
