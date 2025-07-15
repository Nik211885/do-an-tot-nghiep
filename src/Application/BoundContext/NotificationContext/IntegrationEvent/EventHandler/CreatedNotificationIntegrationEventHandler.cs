using Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;
using Application.BoundContext.ModerationContext.IntegrationEvent.Event;
using Application.BoundContext.NotificationContext.Command;
using Application.BoundContext.NotificationContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;
using Application.Interfaces.IdentityProvider;
using Core.BoundContext.NotificationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.NotificationContext.IntegrationEvent.EventHandler;

public class CreatedNotificationIntegrationEventHandler(
    ILogger<CreatedNotificationIntegrationEventHandler> logger,
    IFactoryHandler factoryHandler,
    IIdentityProviderServices identityProviderServices)
    : IIntegrationEventHandler<SubmittedAndReviewedChapterVersionIntegrationEvent>,
        IIntegrationEventHandler<ActivatedBookIntegrationEvent>,
        IIntegrationEventHandler<ApprovedChapterIntegrationEvent>,
        IIntegrationEventHandler<CreatedChapterApprovalIntegrationEvent>,
        IIntegrationEventHandler<OpenedApprovalChapterIntegrationEvent>,
        IIntegrationEventHandler<RejectedChapterIntegrationEvent>,
        IIntegrationEventHandler<UnactivatedBookIntegrationEvent>
{
    private readonly ILogger<CreatedNotificationIntegrationEventHandler> _logger = logger;
    private readonly IFactoryHandler _factoryHandler = factoryHandler;
    private readonly IIdentityProviderServices _identityProviderServices = identityProviderServices;
    public Task Handle(SubmittedAndReviewedChapterVersionIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task Handle(ActivatedBookIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task Handle(ApprovedChapterIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task Handle(CreatedChapterApprovalIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task Handle(OpenedApprovalChapterIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task Handle(RejectedChapterIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task Handle(UnactivatedBookIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    private async Task Handle(string message, Guid authorId, NotificationSubject subject, string title, NotificationChanel chanel)
    {
        var createNotificationCommand = new CreateNotificationCommand(authorId, subject, message, title, chanel);
        await _factoryHandler.Handler<CreateNotificationCommand,NotificationViewModel>(createNotificationCommand);
    }
}
