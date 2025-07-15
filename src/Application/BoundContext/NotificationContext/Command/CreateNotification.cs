using Application.BoundContext.NotificationContext.ViewModel;
using Application.Interfaces.CQRS;
using Core.BoundContext.NotificationContext;
using Core.Interfaces.Repositories.NotificationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.NotificationContext.Command;

public record CreateNotificationCommand(Guid UserId, NotificationSubject Subject, string Message, string Title, NotificationChanel Chanel)
    : INotificationCommand<NotificationViewModel>;

public class CreateNotificationCommandHandler(
    ILogger<CreateNotificationCommandHandler> logger,
    INotificationRepository notificationRepository)
    : ICommandHandler<CreateNotificationCommand, NotificationViewModel>
{
    private readonly ILogger<CreateNotificationCommandHandler> _logger = logger;
    
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    public async Task<NotificationViewModel> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = Notification.Create(request.UserId, request.Subject, request.Message, request.Title, request.Chanel);
        _notificationRepository.CreateNotification(notification);
        _logger.LogInformation("Create notification {@d}", notification.Id);
        await _notificationRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        _logger.LogInformation("Create notification success {@d}", notification.Id);
        return notification.ToViewModel();
    }
}
