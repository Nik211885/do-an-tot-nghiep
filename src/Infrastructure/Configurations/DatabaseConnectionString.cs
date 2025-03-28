namespace Infrastructure.Configurations;
/// <summary>
/// 
/// </summary>
public class DatabaseConnectionString
{
    public string? Master { get; set; }
    public string[]? Slaves { get; set; }
}
