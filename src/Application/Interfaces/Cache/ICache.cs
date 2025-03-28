namespace Application.Interfaces.Cache;
/// <summary>
/// 
/// </summary>
public interface ICache
{
    /// <summary>
    ///     
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<string?> GetAsync(string key);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="leftTime"></param>
    /// <returns></returns>
    Task SetAsync(string key, string value, TimeSpan leftTime);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task RemoveAsync(string key);
    
}
