using Core.Entities;
using Core.Events.NotificationContext;
using Core.Interfaces;

namespace Core.BoundContext.NotificationContext;

public class Notification
    : BaseEntity, IAggregateRoot
{
    public Guid UserId { get; private set; }
    public string Message { get; private set; }
    public string Title { get; private set; }
    public NotificationSubject NotificationSubject { get; private set; }
    public DateTimeOffset CreateNotification { get; private set; }
    public DateTimeOffset SendNotificationDateTime { get; private set; }
    public NotificationChanel  NotificationChanel { get; private set; }
    public NotificationStatus Status { get; private set; }
    protected Notification(){}
    private Notification(Guid userId, NotificationSubject subject, string message,string title, NotificationChanel chanel)
    {
        UserId = userId;
        Message = message;
        Title = title;
        NotificationChanel = chanel;
        NotificationSubject = subject;
        Status = NotificationStatus.Pending;
        RaiseDomainEvent(new CreatedNotificationDomainEvent(this));
    }
    
    public static Notification Create(Guid userId, NotificationSubject subject, string message, string title, NotificationChanel chanel)
    {
        return new Notification(userId, subject, message, title, chanel);
    }

    public void MarkAsSent()
    {
        Status = NotificationStatus.Sent;
        SendNotificationDateTime = DateTimeOffset.UtcNow;
    }

    public void MarkAsFailed()
    {
        Status = NotificationStatus.Failed;
        SendNotificationDateTime = DateTimeOffset.UtcNow;
    }

    public void MarkAsRead()
    {
        Status = NotificationStatus.Read;
    }

    public void DeleteNotification()
    {
        
    }
}
