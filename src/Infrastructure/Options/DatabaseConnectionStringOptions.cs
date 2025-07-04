namespace Infrastructure.Options;
/// <summary>
/// 
/// </summary>
[KeyOptions("DatabaseConnectionString:Postgresql")]
public class DatabaseConnectionStringOptions
{
    public string? Master { get; set; }
    public string[]? Slaves { get; set; }
}
