using Application.Interfaces.CQRS;

namespace Application.BoundContext.NotificationContext.Command;

public interface INotificationCommand<TResponse> : ICommand<TResponse>;
