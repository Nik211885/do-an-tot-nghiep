using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;
/// <summary>
///   Pipeline validation for data it just applies to request is in inherit command
///   It compile with fluent validation 
/// </summary>
/// <param name="validators"></param>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators,
    ILogger<ValidationBehavior<TRequest, TResponse>> logger, IIdentityProvider identityProvider) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    /// <summary>
    /// 
    /// </summary>
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger = logger;
    /// <summary>
    /// 
    /// </summary>
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;
    private readonly IIdentityProvider _identityProvider = identityProvider;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }
        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );
        var  failures = validationResults
            .Where(v=> v.Errors.Count != 0)
            .SelectMany(v => v.Errors)
            .ToList();
        if (failures.Count == 0)
        {
            return await next();
        }

        _logger.LogWarning("Validation failures bad request in request: {request} has userId: {userId} and userName {userName}", request, _identityProvider.UserIdentity(), _identityProvider.UserName());
        throw new ValidationException(failures);
    }
}
