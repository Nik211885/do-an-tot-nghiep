using System.ComponentModel;

namespace Core.BoundContext.NotificationContext;

public enum NotificationSubject
{
    /// <summary>
    /// 
    /// </summary>
    [Description("Kiểm duyệt sách")]
    ModerationBook,
    /// <summary>
    /// 
    /// </summary>
    [Description("Yêu cầu xuất bản sách")]
    RequiredPublishBook,
}
