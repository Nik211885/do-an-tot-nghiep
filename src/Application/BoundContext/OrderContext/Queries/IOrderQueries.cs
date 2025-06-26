using Application.BoundContext.OrderContext.ViewModel;
using Application.Interfaces.Query;
using Application.Models;

namespace Application.BoundContext.OrderContext.Queries;

public interface IOrderQueries : IApplicationQueryServicesExtension
{
    Task<OrderViewModel?> GetOrderHasInBookIdAsync(Guid bookId, CancellationToken cancellationToken);
    Task<PaginationItem<OrderViewModel>> GetOrderForUserWithPaginationAsync(Guid userId,PaginationRequest page, CancellationToken cancellationToken = default);
}
