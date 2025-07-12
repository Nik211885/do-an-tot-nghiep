using Application.BoundContext.OrderContext.ViewModel;
using Application.Interfaces.Query;
using Application.Models;
using Core.BoundContext.OrderContext.OrderAggregate;

namespace Application.BoundContext.OrderContext.Queries;

public interface IOrderQueries : IApplicationQueryServicesExtension
{
    Task<OrderViewModel?> GetOrderHasInBookIdAsync(Guid userId, Guid bookId, CancellationToken cancellationToken);
    Task<PaginationItem<OrderViewModel>> GetOrderForUserWithPaginationAsync(Guid userId,PaginationRequest page, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<OrderViewModel>> GetAllOrderByBookIdsAsync(
        IEnumerable<Guid> ids,
        Guid userId,
        OrderStatus? status,
        CancellationToken cancellationToken = default);
    
    // Queries for statistical payment in system
    Task<StatisticalPaymentViewModel> GetStatisticalPaymentAsync(Guid? userId, CancellationToken cancellationToken = default);
    Task<StatisticalBookPaymentViewModel> GetStatisticalPaymentForBookAsync(Guid? userId, Guid bookId, CancellationToken cancellationToken = default);
}
