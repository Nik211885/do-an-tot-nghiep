using System.Reflection;
using Application.Interfaces.CQRS;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services;


public static class AddCqrsExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly"></param>
    /// <param name="leftTime"></param>
    /// <returns></returns>
    public static IServiceCollection AddHandler(this IServiceCollection services, Assembly? assembly = null, ServiceLifetime leftTime = ServiceLifetime.Scoped)
        => AddScanService(services, typeof(IHandler<,>), assembly, leftTime);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly"></param>
    /// <param name="leftTime"></param>
    /// <returns></returns>
    public static IServiceCollection AddDomainEvents(this IServiceCollection services, Assembly? assembly = null, ServiceLifetime leftTime = ServiceLifetime.Scoped)
        => AddScanService(services, typeof(IDomainEventHandler<>), assembly, leftTime);

    private static IServiceCollection AddScanService(this IServiceCollection services, Type typeInterface, Assembly? assembly = null, ServiceLifetime leftTime = ServiceLifetime.Scoped)
    {
        assembly = assembly ?? Assembly.GetCallingAssembly();
        var servicesScanTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false })
            .SelectMany(t=>t.GetInterfaces())
            .Where(t=>t.IsGenericType && t.GetGenericTypeDefinition() == typeInterface)
            .Select(t=>new { InterfaceType = t, ImplementationType = t });
        foreach (var serviceScanType in servicesScanTypes)
        {
            var descriptor = new ServiceDescriptor(serviceScanType.InterfaceType, serviceScanType.ImplementationType, leftTime);
            services.Add(descriptor);
        }
        return services;
    }
}
