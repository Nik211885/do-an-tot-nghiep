using Application.Interfaces.CQRS;
using Core.Interfaces.Repositories.NotificationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.NotificationContext.Command;

public record MarkAllNotificationNotReadForUserCommand(Guid UserId)
    : INotificationCommand<bool>;

public class MarkAllNotificationNotReadForUserCommandHandler(
    ILogger<MarkAllNotificationNotReadForUserCommandHandler> logger,
    INotificationRepository repository)
    : ICommandHandler<MarkAllNotificationNotReadForUserCommand, bool>
{
    private readonly ILogger<MarkAllNotificationNotReadForUserCommandHandler> _logger = logger;
    private readonly INotificationRepository _repository = repository;
    public async Task<bool> Handle(MarkAllNotificationNotReadForUserCommand request, CancellationToken cancellationToken)
    {
        var notificationNotReadForUser = await _repository
            .GetAllNotificationNotReadForUserAsync(request.UserId, cancellationToken);
        Parallel.ForEach(notificationNotReadForUser, notification=>
        {
            notification.MarkAsRead();
        });
        _logger.LogInformation("Mark all notification for user has {@Id} ", request.UserId);
        var result = _repository.UpdateBulkNotification(notificationNotReadForUser);
        return result;

    }
}
