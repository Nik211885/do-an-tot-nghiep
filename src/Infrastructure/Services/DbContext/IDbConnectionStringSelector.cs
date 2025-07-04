namespace Infrastructure.Services.DbContext;

public interface IDbConnectionStringSelector
{
    /// <summary>
    ///     Get master db connection string
    /// </summary>
    /// <returns></returns>
    string? GetMasterDbConnectionString();
    /// <summary>
    ///     Get slave db connection string
    /// </summary>
    /// <returns></returns>
    string? GetSlaveDbConnectionString();
}
