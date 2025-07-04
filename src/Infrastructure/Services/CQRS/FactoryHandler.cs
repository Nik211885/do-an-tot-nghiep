using Application.Interfaces.CQRS;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services.CQRS;
/// <summary>
/// 
/// </summary>
/// <param name="serviceProvider"></param>
public class FactoryHandler(IServiceProvider serviceProvider) : IFactoryHandler
{
    /// <summary>
    /// 
    /// </summary>
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public async Task<TResponse> Handler<TRequest, TResponse>(TRequest request,
        CancellationToken cancellationToken = default) 
        where TRequest : IRequest<TResponse>
    {
        var handler = _serviceProvider.GetService<IHandler<TRequest, TResponse>>();
        ArgumentNullException.ThrowIfNull(handler);
        var pipeLineBehaviors = _serviceProvider
            .GetServices<IPipelineBehavior<TRequest, TResponse>>()
            .Reverse();
        RequestHandlerDelegate<TResponse> next = () => handler.Handle(request, cancellationToken);
        foreach (var pipeLineBehavior in pipeLineBehaviors)
        {
            var currentNext = next;
            next = () => pipeLineBehavior.Handle(request, currentNext, cancellationToken);
        }
        return await next();
    }
}
