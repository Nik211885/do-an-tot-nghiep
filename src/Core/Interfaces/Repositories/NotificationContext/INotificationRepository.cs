using Core.BoundContext.NotificationContext;

namespace Core.Interfaces.Repositories.NotificationContext;

public interface INotificationRepository : IRepository<Notification>
{
    Task<Notification?> GetNotificationByIdAsync(Guid id, CancellationToken cancellationToken);
    Notification CreateNotification(Notification notification);
    Notification UpdateNotification(Notification notification);
    bool UpdateBulkNotification(List<Notification> notifications);
    void DeleteNotification(Notification notification);
    Task<List<Notification>> GetAllNotificationNotReadForUserAsync(Guid userId, CancellationToken cancellationToken);
    Task<List<Notification>> GetNotificationsAsync(List<Guid> ids, CancellationToken cancellationToken);
}
