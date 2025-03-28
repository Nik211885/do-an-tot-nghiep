using Application.Interfaces.CQRS;
using FluentValidation;

namespace Application.Behaviors;
/// <summary>
///   Pipeline validation for data it just applies to request is in inherit command
///   It compile with fluent validation 
/// </summary>
/// <param name="validators"></param>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;
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
        if (failures.Count != 0)
        {
            throw new ValidationException(failures);
        }
        return await next();
    }
}
