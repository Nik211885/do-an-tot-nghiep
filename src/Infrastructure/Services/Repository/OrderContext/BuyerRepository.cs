using Core.BoundContext.OrderContext.BuyerAggregate;
using Core.Interfaces.Repositories.OrderContext;
using Infrastructure.Data.DbContext;

namespace Infrastructure.Services.Repository.OrderContext;

public class BuyerRepository(OrderDbContext orderDbContext) 
    :  Repository<Buyer>(orderDbContext), IBuyerRepository
{
    private readonly OrderDbContext _orderDbContext = orderDbContext;
}
