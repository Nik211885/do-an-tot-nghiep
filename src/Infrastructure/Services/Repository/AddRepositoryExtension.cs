using System.Reflection;
using Core.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services.Repository;

public static class AddRepositoryExtension
{
    /// <summary>
    ///     Add all dependency to repository 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblyInterfaceType">Assembly make scan repository type</param>
    /// <param name="assemblyImplementationType">Assembly make scan repository type</param>
    /// <param name="leftTime">Services left time</param>
    /// <returns></returns>
    public static IServiceCollection AddRepository(this IServiceCollection services,
        Assembly assemblyInterfaceType,
        Assembly assemblyImplementationType,
        ServiceLifetime leftTime = ServiceLifetime.Scoped)
    {
        ArgumentNullException.ThrowIfNull(assemblyInterfaceType);
        ArgumentNullException.ThrowIfNull(assemblyImplementationType);
        
        // Get interface repository base in repository implement repository base
        var interfaceRepositoryTypes = assemblyInterfaceType.GetTypes()
            .Where(t => t.IsInterface && t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>)));
        
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
