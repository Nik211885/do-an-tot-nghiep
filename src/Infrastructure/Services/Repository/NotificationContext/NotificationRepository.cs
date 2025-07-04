using Core.BoundContext.NotificationContext;
using Core.Interfaces.Repositories.NotificationContext;
using EFCore.BulkExtensions;
using Elastic.Clients.Elasticsearch.Snapshot;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repository.NotificationContext;

public class NotificationRepository(NotificationDbContext context)
    : Repository<Core.BoundContext.NotificationContext.Notification>(context),
        INotificationRepository
{
    private readonly NotificationDbContext  _context = context;
    public async Task<Core.BoundContext.NotificationContext.Notification?> GetNotificationByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var notificationById = await _context.Notifications
            .FirstOrDefaultAsync(x=>x.Id == id, cancellationToken);
        return notificationById;
    }

    public Core.BoundContext.NotificationContext.Notification CreateNotification(Core.BoundContext.NotificationContext.Notification notification)
    {
        return _context.Notifications.Add(notification).Entity;
    }

    public Core.BoundContext.NotificationContext.Notification UpdateNotification(Core.BoundContext.NotificationContext.Notification notification)
    {
        _context.Notifications.Update(notification);
        return notification;
    }

    public bool UpdateBulkNotification(List<Core.BoundContext.NotificationContext.Notification> notifications)
    {
        _context.BulkUpdate(notifications);
        return true;
    }

    public void DeleteNotification(Core.BoundContext.NotificationContext.Notification notification)
    {   
        _context.Notifications.Remove(notification);
    }

    public async Task<List<Core.BoundContext.NotificationContext.Notification>> GetAllNotificationNotReadForUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var notificationForUserNotRead = await _context.Notifications
            .Where(x=>x.UserId == userId  && x.Status != NotificationStatus.Read)
            .ToListAsync(cancellationToken);
        return notificationForUserNotRead;
    }

    public async Task<List<Core.BoundContext.NotificationContext.Notification>> GetNotificationsAsync(List<Guid> ids, CancellationToken cancellationToken)
    {
        var notifications = await _context.Notifications
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);
        return notifications;   
    }
}
