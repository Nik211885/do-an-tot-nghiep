using System.Diagnostics;
using Application.Interfaces.CQRS;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;
/// <summary>
///     
/// </summary>
/// <param name="logger"></param>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class PerformanceBehavior<TRequest, TResponse>(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger = logger;
    private readonly Stopwatch _stopwatch = new Stopwatch();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _stopwatch.Start();
        var response = await next();
        _stopwatch.Stop();
        var elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
        _logger.LogWarning("Performance with request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}", typeof(TRequest).Name, elapsedMilliseconds, request);
        return response;
    }
}
