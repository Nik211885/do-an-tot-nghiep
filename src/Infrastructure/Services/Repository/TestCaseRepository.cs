using Core.Entities.TestAggregate;
using Core.Interfaces.Repositories;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Helper;
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

    public async Task<PaginationItem<TestCaseAggregate>> GetTestCaseWithPaginationAsync(int pageNumber, int pageSize)
        => await _dbContext.TestCaseAggregate.AsNoTracking().CreatePaginationAsync(pageNumber, pageSize);
}
