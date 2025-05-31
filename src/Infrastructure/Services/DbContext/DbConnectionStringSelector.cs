using Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.DbContext;

public class DbConnectionStringSelector(IOptions<DatabaseConnectionStringOptions> optionDatabaseConnectionStrings) 
    : IDbConnectionStringSelector
{
    private readonly DatabaseConnectionStringOptions _connectionStringOptions = optionDatabaseConnectionStrings.Value;
    private int _currentSlaveDbIndex = 0;
    /// <summary>
    ///  In architecture, I have set up one master database
    /// </summary>
    /// <returns></returns>
    public string? GetMasterDbConnectionString()
        => _connectionStringOptions.Master;
    /// <summary>
    ///     Get slave db with algorithm round-robin
    /// </summary>
    /// <returns>
    ///     Return slave connection string for slave db
    /// if slave is not exits it return master connection string
    /// </returns>
    public string? GetSlaveDbConnectionString()
    {
        var slaveConnectionStrings = _connectionStringOptions.Slaves;
        if (slaveConnectionStrings is null || slaveConnectionStrings.Length == 0)
        {
            return _connectionStringOptions.Master;
        }
        var slaveConnectionString = slaveConnectionStrings.ElementAt(_currentSlaveDbIndex);
        _currentSlaveDbIndex = (_currentSlaveDbIndex + 1) % slaveConnectionStrings.Length;
        return slaveConnectionString;
    }
}
