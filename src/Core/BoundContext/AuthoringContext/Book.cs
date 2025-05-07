using Core.Entities;
using Core.Interfaces;

namespace Core.BoundContext.BookManagement.BookAggregate;
/// <summary>
/// 
/// </summary>
public class Book 
    : BaseEntity, IAggregateRoot
{
    /// <summary>
    /// 
    /// </summary>
    public string Title { get; private set; } = null!;
    /// <summary>
    /// 
    /// </summary>

    public string? AvatarUrl { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Description { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTimeOffset CreatedDateTime { get;private set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTimeOffset LastUpdateDateTime { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTimeOffset PublishDateTime { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public bool IsComplete { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public int VersionNumber { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public bool Visibility { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public PolicyReadBook PolicyReadBook { get; private set; } = null!;
    /// <summary>
    /// 
    /// </summary>
    public BookReleaseType BookReleaseType { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public BookStatus BookStatus { get; private set; }
}
