using Core.BoundContext.NotificationContext;

namespace Domain.UnitTest.BoundContext.NotificationContext;

public class NotificationTests
{
    [Fact]
    public void Create_Should_Succeed_With_Valid_Data()
    {
        var userId = Guid.NewGuid();
        var notification = Notification.Create(userId, NotificationSubject.ModerationBook, "msg", "title", NotificationChanel.Email);
        Assert.Equal(userId, notification.UserId);
        Assert.Equal(NotificationSubject.ModerationBook, notification.NotificationSubject);
        Assert.Equal("msg", notification.Message);
        Assert.Equal("title", notification.Title);
        Assert.Equal(NotificationChanel.Email, notification.NotificationChanel);
        Assert.Equal(NotificationStatus.Pending, notification.Status);
    }

    [Fact]
    public void MarkAsSent_Should_Set_Status_And_SendDate()
    {
        var notification = Notification.Create(Guid.NewGuid(), NotificationSubject.ModerationBook, "msg", "title", NotificationChanel.Email);
        notification.MarkAsSent();
        Assert.Equal(NotificationStatus.Sent, notification.Status);
        Assert.True(notification.SendNotificationDateTime <= DateTimeOffset.UtcNow);
    }

    [Fact]
    public void MarkAsFailed_Should_Set_Status_And_SendDate()
    {
        var notification = Notification.Create(Guid.NewGuid(), NotificationSubject.ModerationBook, "msg", "title", NotificationChanel.Email);
        notification.MarkAsFailed();
        Assert.Equal(NotificationStatus.Failed, notification.Status);
        Assert.True(notification.SendNotificationDateTime <= DateTimeOffset.UtcNow);
    }

    [Fact]
    public void MarkAsRead_Should_Set_Status_Read()
    {
        var notification = Notification.Create(Guid.NewGuid(), NotificationSubject.ModerationBook, "msg", "title", NotificationChanel.Email);
        notification.MarkAsRead();
        Assert.Equal(NotificationStatus.Read, notification.Status);
    }
}
