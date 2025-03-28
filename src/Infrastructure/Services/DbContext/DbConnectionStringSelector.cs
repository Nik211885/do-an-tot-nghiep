using Infrastructure.Configurations;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.DbContext;

public class DbConnectionStringSelector(IOptions<DatabaseConnectionString> optionDatabaseConnectionStrings) 
    : IDbConnectionStringSelector
{
    private readonly DatabaseConnectionString _connectionString = optionDatabaseConnectionStrings.Value;
    private int _currentSlaveDbIndex = 0;
    /// <summary>
    ///  In architecture, I have set up one master database
    /// </summary>
    /// <returns></returns>
    public string? GetMasterDbConnectionString()
        => _connectionString.Master;
    /// <summary>
    ///     Get slave db with algorithm round-robin
    /// </summary>
    /// <returns>
    ///     Return slave connection string for slave db
    /// if slave is not exits it return master connection string
    /// </returns>
    public string? GetSlaveDbConnectionString()
    {
        var slaveConnectionStrings = _connectionString.Slaves;
        if (slaveConnectionStrings is null || slaveConnectionStrings.Length == 0)
        {
            return _connectionString.Master;
        }
        var slaveConnectionString = slaveConnectionStrings.ElementAt(_currentSlaveDbIndex);
        _currentSlaveDbIndex = (_currentSlaveDbIndex + 1) % slaveConnectionStrings.Length;
        return slaveConnectionString;
    }
}
