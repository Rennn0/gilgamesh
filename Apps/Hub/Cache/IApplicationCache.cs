namespace Hub.Cache;

public interface IApplicationCache
{
    Task<T?> GetAsync<T>(string key);
    Task<bool> SetAsync(string key, object data);
    Task<bool> SetAsync(string key, object data, bool replace);
    Task<bool> SetAsync(string key, object data, TimeSpan expiration);
    Task<bool> SetAsync(string key, object data, bool replace, TimeSpan expiration);
    Task<bool> DeleteAsync(string key);
}
