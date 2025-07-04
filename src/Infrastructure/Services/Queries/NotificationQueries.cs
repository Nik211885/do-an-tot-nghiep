using Application.BoundContext.NotificationContext.Queries;
using Application.BoundContext.NotificationContext.ViewModel;
using Application.Models;
using Infrastructure.Data.DbContext;
using Infrastructure.Helper;

namespace Infrastructure.Services.Queries;

public class NotificationQueries(NotificationDbContext context)
    : INotificationQueries
{
    private readonly NotificationDbContext _context = context;

    public async Task<PaginationItem<NotificationViewModel>> GetNotificationForUserWithPaginationAsync(Guid userId, PaginationRequest page,
        CancellationToken cancellationToken = default)
    {
        var notifications = await _context.Notifications
            .Where(x => x.UserId == userId)
            .CreatePaginationAsync(page, (notification) =>
            new NotificationViewModel(
                    notification.Id,
                    notification.UserId,
                    notification.Message,
                    notification.Title,
                    notification.NotificationSubject,
                    notification.CreateNotification,
                    notification.SendNotificationDateTime,
                    notification.NotificationChanel,
                    notification.Status
                ),
                cancellationToken);
        return notifications;
    }
}
