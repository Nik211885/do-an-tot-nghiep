using Application.BoundContext.NotificationContext.ViewModel;
using Application.Interfaces.Query;
using Application.Models;

namespace Application.BoundContext.NotificationContext.Queries;

public interface INotificationQueries : IApplicationQueryServicesExtension
{
    Task<PaginationItem<NotificationViewModel>> GetNotificationForUserWithPaginationAsync(Guid userId, PaginationRequest page, CancellationToken cancellationToken = default);
}
