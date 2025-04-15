using System.Reflection;

namespace PublicAPI.Services.Endpoint;
public static class EndpointsExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpoints"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpoints, Assembly? assembly = null)
    {
        assembly = assembly ?? Assembly.GetCallingAssembly();
        var typeIEndpoints = typeof(IEndpoints);
        var endPointTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && typeIEndpoints.IsAssignableFrom(t));
        foreach (var endPointType in endPointTypes)
        {
            var endpointInstance = Activator.CreateInstance(endPointType) as IEndpoints;
            ArgumentNullException.ThrowIfNull(endpointInstance);
            endpointInstance.Map(endpoints);
        }
        return endpoints;
    }
}
