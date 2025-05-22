namespace Application.Interfaces.IntegrationEven;
//IntegrationEvent
/// <summary>
/// 
/// </summary>
public class IntegrationEven
{
    /// <summary>
    /// 
    /// </summary>
    public Guid Id { get; set; } = Guid.CreateVersion7();
    /// <summary>
    /// 
    /// </summary>
    public DateTimeOffset CreateOn { get; } = DateTimeOffset.UtcNow;
}
