using Application.BoundContext.ModerationContext.IntegrationEvent.Event;
using Application.BoundContext.NotificationContext.Command;
using Application.BoundContext.NotificationContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;
using Core.BoundContext.NotificationContext;

namespace Application.BoundContext.NotificationContext.IntegrationEvent.EventHandler;

public class CreatedNotificationIntegrationEventHandler(IFactoryHandler factoryHandler)
    : IIntegrationEventHandler<ApprovalBookIntegrationEvent>,
        IIntegrationEventHandler<RejectBookIntegrationEvent>
{
    private readonly IFactoryHandler _factoryHandler = factoryHandler;
    public async Task Handle(ApprovalBookIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        string title = @"
                        Kiểm duyệt sách thành công
                    ";
        var message = @"Chúng tôi đã xem xét tác phầm của bạn,
                    tác phầm hoàn toàn đạt các tiêu chí quy tắc cộng đồng 
                     và đã được hiển thị lên trang chủ của chúng tôi
                    Cảm ơn bạn rất nhiều vì đã chọn nền tảng của chúng tôi để 
                    viết những tác phẩm của bạn, 
                    Nếu bạn có vấn đề hay thắc mặc gì hãy liên hệ với chúng tôi";
        var notificationCreate = new  CreateNotificationCommand(@event.ChapterUserId, NotificationSubject.ModerationBook, message, title, NotificationChanel.All);
        await _factoryHandler.Handler<CreateNotificationCommand, NotificationViewModel>(notificationCreate, cancellationToken);
    }

    public async Task Handle(RejectBookIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        string title = @"
                        Kiểm duyệt sách thất bại
                    ";
        var message = @$"Chúng tôi đã xem xét tác phầm của bạn,
                      Tác phẩm của bạn {@event.Note}
                    Cảm ơn bạn rất nhiều vì đã chọn nền tảng của chúng tôi để 
                    viết những tác phẩm của bạn, 
                    Nếu bạn có vấn đề hay thắc mặc gì hãy liên hệ với chúng tôi";
        var notificationCreate = new  CreateNotificationCommand(@event.ChapterUserId, NotificationSubject.ModerationBook, message, title, NotificationChanel.All);
        await _factoryHandler.Handler<CreateNotificationCommand, NotificationViewModel>(notificationCreate, cancellationToken);
    }
}
