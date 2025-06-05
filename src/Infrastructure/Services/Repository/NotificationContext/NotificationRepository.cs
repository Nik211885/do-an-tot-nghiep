using Core.Interfaces.Repositories.NotificationContext;
using Infrastructure.Data.DbContext;

namespace Infrastructure.Services.Repository.NotificationContext;

public class NotificationRepository(NotificationDbContext context)
    : Repository<Core.BoundContext.NotificationContext.Notification>(context),
        INotificationRepository
{
}
