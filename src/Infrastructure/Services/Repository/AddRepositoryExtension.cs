using System.Reflection;
using Application.Interfaces.Query;
using Core.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services.Repository;

public static class AddRepositoryExtension
{
    public static IServiceCollection AddApplicationServicesExtension(this IServiceCollection services)
        => services.AddScanTypeToBaseAbstraction(typeof(IApplicationQueryServicesExtension));
    public static IServiceCollection AddRepository(this IServiceCollection services)
        => services.AddScanTypeToBaseAbstraction(typeof(IRepository<>));
    
    /// <summary>
    ///     Add all dependency to repository 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblyImplementationType">Assembly make scan repository type</param>
    /// <param name="typeInterfaceTypeScan"></param>
    /// <param name="leftTime">Services left time</param>
    /// <returns></returns>
    private static IServiceCollection AddScanTypeToBaseAbstraction(this IServiceCollection services,
        Type typeInterfaceTypeScan,
        Assembly? assemblyImplementationType = null,
        ServiceLifetime leftTime = ServiceLifetime.Scoped)
    {
        ArgumentNullException.ThrowIfNull(typeInterfaceTypeScan);
        assemblyImplementationType ??= Assembly.GetExecutingAssembly();
        var assemblyInterfaceType = typeInterfaceTypeScan.Assembly;
        // Get interface repository base in repository implement repository base
        var interfaceRepositoryTypes = assemblyInterfaceType.GetTypes()
            .Where(t => t.IsInterface && t.GetInterfaces()
                .Any(i => (typeInterfaceTypeScan.IsGenericTypeDefinition && i.IsGenericType &&
                           i.GetGenericTypeDefinition() == typeInterfaceTypeScan)
                          || (!typeInterfaceTypeScan.IsGenericTypeDefinition && i == typeInterfaceTypeScan)
                ));
        // get all implement interface repository and add to dictionary
        var implementationRepositoryTypes = assemblyImplementationType.GetTypes()
            .Where(t=>t is{IsClass:true, IsAbstract:false})
            .SelectMany(impl => impl.GetInterfaces().Select(i=>new { InterfaceType = i, ImplementationType = impl }))
            .Where(x=> interfaceRepositoryTypes.Contains(x.InterfaceType))
            .ToDictionary(x=>x.InterfaceType, x=>x.ImplementationType);
        foreach (var (interfaceType, implementInterfaceType) in implementationRepositoryTypes)
        {
            var serviceDescriptor = new ServiceDescriptor(interfaceType, implementInterfaceType, leftTime);
            services.Add(serviceDescriptor);
        }
        return services;
    }
}
