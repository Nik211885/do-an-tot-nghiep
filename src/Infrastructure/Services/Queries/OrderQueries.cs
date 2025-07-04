using Application.BoundContext.OrderContext.Queries;
using Application.BoundContext.OrderContext.ViewModel;
using Application.Models;
using Core.BoundContext.OrderContext.OrderAggregate;
using Infrastructure.Data.DbContext;
using Infrastructure.Helper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Queries;

public class OrderQueries(OrderDbContext orderDbContext) 
    : IOrderQueries
{
    private readonly OrderDbContext _orderDbContext = orderDbContext;
    public async Task<OrderViewModel?> GetOrderHasInBookIdAsync(Guid userId, Guid bookId, CancellationToken cancellationToken)
    {
        var order = await _orderDbContext.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderItems.Any(oi => oi.BookId == bookId)
                                      && o.BuyerId == userId,
                cancellationToken);

        return order?.ToViewModel();
    }

    public async Task<PaginationItem<OrderViewModel>> GetOrderForUserWithPaginationAsync(Guid userId, PaginationRequest page, CancellationToken cancellationToken = default)
    {
        var ordersWithPagination
            = await _orderDbContext.Orders
                .AsNoTracking()
                .Where(x => x.BuyerId == userId)
                .Include(x=>x.OrderItems)
                .CreatePaginationAsync(page, order => new OrderViewModel(
                        order.Id,
                        order.OrderDate,
                        order.BuyerId,
                        order.Status,
                        order.OrderItems.Select(x=>new OrderItemViewModel(
                            x.Id,
                            x.BookId,
                            x.BookName,
                            x.Price
                            )).ToList()
                        ),
                    cancellationToken);
        return ordersWithPagination;
    }

    public async Task<IReadOnlyCollection<OrderViewModel>> 
        GetAllOrderByBookIdsAsync(IEnumerable<Guid> ids,
            Guid userId,
            OrderStatus? status,
            CancellationToken cancellationToken =  default)
    {
        var query = 
            _orderDbContext.Orders
                .AsNoTracking()
                .Include(x=>x.OrderItems)
                .Where(x=>x.OrderItems
                    .Any(y=>ids.Contains(y.BookId))
                &&(status == null || x.Status == status)
                &&  x.BuyerId == userId);
        var result = await query.ToListAsync(cancellationToken);
        return result.Select(x=>x.ToViewModel()).ToList();
    }
}
