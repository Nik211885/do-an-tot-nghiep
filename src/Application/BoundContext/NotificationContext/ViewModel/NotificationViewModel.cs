using Core.BoundContext.NotificationContext;

namespace Application.BoundContext.NotificationContext.ViewModel;

public class NotificationViewModel
{
    public Guid Id { get; }
    public Guid UserId { get; }
    public string Message { get; }
    public string Title { get; }
    public NotificationSubject Subject { get; }
    public DateTimeOffset CreateNotification { get; }
    public DateTimeOffset SendNotificationDateTime { get; }
    public NotificationChanel  NotificationChanel { get; }
    public NotificationStatus Status { get; }

    public NotificationViewModel(Guid id, Guid userId, string message, string title, NotificationSubject subject, DateTimeOffset createNotification, DateTimeOffset sendNotificationDateTime, NotificationChanel notificationChanel, NotificationStatus status)
    {
        Id = id;
        UserId = userId;
        Message = message;
        Title = title;
        Subject = subject;
        CreateNotification = createNotification;
        SendNotificationDateTime = sendNotificationDateTime;
        NotificationChanel = notificationChanel;
        Status = status;
    }
}


public static class NotificationMappingExtension
{
    public static NotificationViewModel ToViewModel(this Notification notification)
    {
        return new NotificationViewModel(
            id: notification.Id,
            userId: notification.UserId,
            message: notification.Message,
            title: notification.Title,
            subject: notification.NotificationSubject,
            createNotification: notification.CreateNotification,
            sendNotificationDateTime: notification.SendNotificationDateTime,
            notificationChanel: notification.NotificationChanel,
            status: notification.Status
        );
    }

    public static IReadOnlyCollection<NotificationViewModel> ToViewModel(this List<Notification> notifications)
    {
        return notifications.Select(ToViewModel).ToList();
    }
}
