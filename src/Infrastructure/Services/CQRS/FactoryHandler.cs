using Application.Interfaces.CQRS;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services.CQRS;

public class FactoryHandler(IServiceProvider serviceProvider) : IFactoryHandler
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    public async Task<TResponse> Handler<TRequest, TResponse>(TRequest request,
        CancellationToken cancellationToken) 
        where TRequest : IRequest<TResponse>
    {
        var pipeLineBehaviors = _serviceProvider
            .GetServices<IPipelineBehavior<TRequest, TResponse>>();
        var handler = _serviceProvider.GetService<IHandler<TRequest, TResponse>>();
        ArgumentNullException.ThrowIfNull(handler);
        RequestHandlerDelegate<TResponse> next = () => handler.Handle(request, cancellationToken);
        foreach (var pipeLineBehavior in pipeLineBehaviors)
        {
            var currentNext = next;
            next = () => pipeLineBehavior.Handle(request, currentNext, cancellationToken);
        }
        return await next();
    }
}
