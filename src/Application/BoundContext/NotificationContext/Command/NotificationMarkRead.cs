using Application.Interfaces.CQRS;
using Core.Interfaces.Repositories.NotificationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.NotificationContext.Command;

public record NotificationMarkReadCommand(params Guid[] Id)
    : INotificationCommand<bool>;

public class NotificationMarkReadCommandHandler(
    ILogger<NotificationMarkReadCommandHandler> logger,
    INotificationRepository notificationRepository)
    : ICommandHandler<NotificationMarkReadCommand, bool>
{
    public async Task<bool> Handle(NotificationMarkReadCommand request, CancellationToken cancellationToken)
    {
        var notifications =
            await notificationRepository.GetNotificationsAsync(request.Id.ToList(), cancellationToken);
        logger.LogInformation("Start mark notification read with id is {@Ids}", request.Id);
        Parallel.ForEach(notifications, n =>
        {
            n.MarkAsRead();
        });
        var result = notificationRepository.UpdateBulkNotification(notifications);
        return result;
    }
}
