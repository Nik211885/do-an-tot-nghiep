namespace Core.BoundContext.NotificationContext;

public enum NotificationStatus
{
    /// <summary>
    ///     When message has created 
    /// </summary>
    Pending,
    /// <summary>
    ///  When message has send success
    /// </summary>
    Sent,
    /// <summary>
    ///  When user has mark message has 
    /// </summary>
    Read,
    /// <summary>
    ///  When message faild send to user
    /// </summary>
    Failed
}
