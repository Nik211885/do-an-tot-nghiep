## Think about application layer


In application layer is defined services for Persistence layer want to use abstraction layer to services,
and it has implement in inf layer

What do you think about problem is pagination it has in application services because core layer 
don't need page size and page number in ui layer want

But some time handler just call services add don't add any logic.
What do you think about every service is disposed in Persistence or just is handler it makes complex code,
but it makes simple design and easy test, and it makes not good for performance application


### Problem about think

I want to make don't have dependency redundant in between layer like in domain and application I don't expose queryable
it dependency to linq and ef I know you convert expression to sql and still query in database but in my case I don't like make this


## Concept about module

It encapsulation to folder it makes to easy fun file release make edit


## Think 
If you use command with mediator and query is interface services in application and implement in ifra layer,
but it has problem when I need add services like cache it want to appy open close prince just has use wraper interface
Make good performance i just apply query is use cqrs if query need more services like 
cache forever i just use query through abstraction query extension implement in inf layer
```csharp
    interface IOrderQueryService
    {
        Task<IReadOnlyCollection<Reponse>> GetOrderByUserName(string userName);
    }
```
implement services
```csharp
    class OrderQueryServices : IOrderQueryService
    {
        private readonly OrderDbContext _orderDbContex;
        public OrderQueryServices(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext
        }
        public async Task<IReadOnlyCollection<Response> GetOrderByUserName(string name)
        {
            var result = await _orderDbContext.Order.Where(x=>x.UserName == userName);
            return result;
        }
    }
```
 
but If i want to add cache to services i make wrapper old services

```csharp
    class OrderQueryWithCaheServices : IOrderQueryService
    {
        private IOrderQueryService _oldOrderServices;
        private ICache _cache;
        public OrderQueryWithCaheServices(IOrderQueryService orderQueryService, ICache cache)
        {
            _oldOrderServices = orderService;
            _cache = cache;
        }
        public async Task<IReadOnlyCollection<Response> GetOrderByUserName(string name)
        {
            var key = $"prduct{username}";
            var product = await _cache.GetValue(key);
            if(!product.IsEmpty)
            {
                return product;
            }
            product = _oldOrderServices.GetOrderByUserName(userName);
            _cache.SetValue(product);
            return product;
        }
    }
```

In Services Container

```csharp
services.AddScope(OrderQueryServices);
services.AddSingleton<ICache,Cahe>();
services.AddScope<IOrderQueryServices>(provider=>
{
    var oldOrderQueryServices = provider.GetRequiredService<OrderQueryServices>;
    var oldOrderQueryServices = provider.GetRequiredService<ICache>();
    return new OrderQueryWithCaheServices(oldOrderQueryServices,cache);
})
```

Think about shared layer in between bound context

## Integration event