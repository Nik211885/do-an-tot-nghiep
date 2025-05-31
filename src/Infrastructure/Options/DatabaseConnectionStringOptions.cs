namespace Infrastructure.Options;
/// <summary>
/// 
/// </summary>
public class DatabaseConnectionStringOptions
{
    public string? Master { get; set; }
    public string[]? Slaves { get; set; }
}
