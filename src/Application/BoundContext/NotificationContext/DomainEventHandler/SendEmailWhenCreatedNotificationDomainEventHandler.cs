using Application.Helper;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Application.Interfaces.Notification;
using Core.BoundContext.NotificationContext;
using Core.Events.NotificationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.NotificationContext.DomainEventHandler;

public class SendEmailWhenCreatedNotificationDomainEventHandler(
    IEmailSender sender,
    IIdentityProviderServices identityProviderServices,
    ILogger<SendEmailWhenCreatedNotificationDomainEventHandler> logger)
    : IEventHandler<CreatedNotificationDomainEvent>
{
    private readonly IEmailSender _sender = sender;
    private readonly ILogger<SendEmailWhenCreatedNotificationDomainEventHandler> _logger = logger;
    private readonly IIdentityProviderServices _identityProviderServices = identityProviderServices;
    public async Task Handler(CreatedNotificationDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var notification = domainEvent.Notification;
        if (notification.NotificationChanel != NotificationChanel.Email)
        {
            return;
        }

        var userInfo = await _identityProviderServices.GetUserInfoAsync(notification.UserId.ToString());
        if (userInfo is not null)
        {
            await _sender.SendEmailAsync(userInfo.Email, notification.Message,
                notification.NotificationSubject.GetDescriptionAttribute(), null);
        }
        else
        {
            _logger.LogError("User {@UserId} don't have email so dont send email notification", notification.UserId);
        }
    }
}
