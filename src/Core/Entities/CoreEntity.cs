namespace Core.Entities;
/// <summary>
///     Core entity inherit entity and additional property created, creation date and updated, update date
/// </summary>
public abstract class CoreEntity : BaseEntity
{
    /// <summary>
    ///     
    /// </summary>
    public DateTimeOffset CreateDateTimeOffset { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTimeOffset UpdateDateTimeOffset { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public Guid CreatedBy { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public Guid UpdatedBy { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    protected CoreEntity() : base()
    {
        
    } 
    /// <summary>
    /// 
    /// </summary>
    /// <param name="createdBy"></param>
    protected CoreEntity(Guid createdBy) : base()
    {
        var dateUtcNow = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
        CreateDateTimeOffset = dateUtcNow;
        UpdatedBy = createdBy;
        UpdateDateTimeOffset = dateUtcNow;
    }
    
}
