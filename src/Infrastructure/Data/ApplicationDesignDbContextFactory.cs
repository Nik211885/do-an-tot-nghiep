using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data;
/// <summary>
/// 
/// </summary>
public class ApplicationDesignDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    /// <summary>
    /// /
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        throw new NotImplementedException();
    }
}
