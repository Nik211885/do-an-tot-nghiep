namespace Application.Interfaces.Cache;
/// <summary>
///     Cache services
/// </summary>
public interface ICache
{
    /// <summary>
    ///    Get data in cache has key
    /// </summary>
    /// <param name="key"></param>
    /// <returns>
    ///     Return value has key if it has exits otherwise return null
    /// </returns>
    Task<string?> GetAsync(string key);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>

    Task<TValue?> GetAsync<TValue>(string key);
    /// <summary>
    ///     Write value in cache has key and left time exits
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expiresIn"></param>
    /// <returns>
    ///     
    /// </returns>
    Task SetAsync(string key, string value, int expiresIn);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expiresIn"></param>
    /// <returns></returns>
    Task SetAsync(string key, object value, int expiresIn);
    /// <summary>
    ///     Remove value has key in cache
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task RemoveAsync(string key);
    
}
