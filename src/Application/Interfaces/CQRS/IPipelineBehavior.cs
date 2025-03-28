namespace Application.Interfaces.CQRS;

/// <summary>
///     Delegate next context in pipeline
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

/// <summary>
///     
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IPipelineBehavior<in TRequest, TResponse> where TRequest 
    : IRequest<TResponse>
{
    /// <summary>
    ///     Execute handler pipeline in application it just applies to command and query handler
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     
    /// </returns>
    Task<TResponse> Handle(TRequest request, 
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken);
}
