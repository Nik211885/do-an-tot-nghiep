using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;
/// <summary>
/// 
/// </summary>
/// <param name="logger"></param>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger,
    IIdentityProvider identityProvider) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    // Log all request and has information about user and action 
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger;
    private readonly IIdentityProvider _identityProvider = identityProvider;
    /// <summary>
    ///     Logging information is star and end request for user
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var attribute =  typeof(TRequest).Attributes;
        _logger.LogInformation("Start logging request: {request}, userId: {userId}, userName {userName}", 
            typeof(TRequest).FullName, _identityProvider.UserIdentity(), _identityProvider.UserName());
        // In natural, you want to pass information about user and request for user
        var response = await next();
        _logger.LogInformation("End logging request: {request}, userid: {uerId}, userName {userName}", 
            typeof(TRequest).FullName,_identityProvider.UserIdentity(), _identityProvider.UserName() );
        return response;
    }
}
