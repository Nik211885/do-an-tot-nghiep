using Core.BoundContext.NotificationContext;

namespace Core.Events.NotificationContext;

public class CreatedNotificationDomainEvent : IEvent
{
    public Notification Notification { get;  }

    public CreatedNotificationDomainEvent(Notification notification)
    {
        Notification = notification;
    }
}

