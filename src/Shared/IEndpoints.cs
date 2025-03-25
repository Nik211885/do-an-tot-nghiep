using Microsoft.AspNetCore.Routing;

namespace Shared;
/// <summary>
///     
/// </summary>
public interface IEndpoints
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpoint"></param>
    void Map(IEndpointRouteBuilder endpoint);
}
