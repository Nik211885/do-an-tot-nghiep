﻿using System.Reflection;
using Application.Interfaces.CQRS;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services;


public static class AddCqrsExtension
{
    /// <summary>
    ///     Add all handler command and query with left time is scope and in base assembly
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly"></param>
    /// <param name="leftTime"></param>
    /// <returns></returns>
    public static IServiceCollection AddHandler(this IServiceCollection services, Assembly? assembly = null, ServiceLifetime leftTime = ServiceLifetime.Scoped)
        => services.AddScanService(typeof(IHandler<,>), assembly, leftTime);
    /// <summary>
    ///     Add all domain event and query with left time is scope and in base assembly
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly"></param>
    /// <param name="leftTime"></param>
    /// <returns></returns>
    public static IServiceCollection AddDomainEvents(this IServiceCollection services, Assembly? assembly = null, ServiceLifetime leftTime = ServiceLifetime.Scoped)
        => services.AddScanService(typeof(IEventHandler<>), assembly, leftTime);

    private static IServiceCollection AddScanService(this IServiceCollection services, Type typeInterface, Assembly? assembly = null, ServiceLifetime leftTime = ServiceLifetime.Scoped)
    {
        assembly = assembly ?? Assembly.GetCallingAssembly();
        var servicesScanTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false })  
            .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeInterface) 
                    .Select(i => new { InterfaceType = i, ImplementationType = t }) 
            );
        foreach (var serviceScanType in servicesScanTypes)
        {
            var descriptor = new ServiceDescriptor(serviceScanType.InterfaceType, serviceScanType.ImplementationType, leftTime);
            services.Add(descriptor);
        }
        return services;
    }
}
