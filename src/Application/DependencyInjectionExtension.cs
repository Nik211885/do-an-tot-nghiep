using System.Reflection;
using Application.Behaviors;
using Application.Interfaces.CQRS;
using Application.Middlewares;
using Application;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Application;

public static class DependencyInjectionExtension
{
    /// <summary>
    ///     
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddDomainEvents(assembly);
        services.AddHandler(assembly);
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjectionExtension));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        services.AddTransient<ExceptionMiddlewareHandling>();
        return services;
    }
}
