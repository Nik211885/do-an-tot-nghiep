using Application.Interfaces.CQRS;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;
/// <summary>
/// 
/// </summary>
/// <param name="logger"></param>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    // Log all request and has information about user and action 
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start logging behavior");
        // In natural, you want to pass information about user and request for user
        return await next();
    }
}
