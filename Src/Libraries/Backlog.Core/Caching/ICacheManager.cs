﻿namespace Backlog.Core.Caching
{
    public interface ICacheManager : IDisposable
    {
        Task<T> GetAsync<T>(string key, Func<Task<T>> acquire, int? cacheTime = null);

        Task SetAsync(string key, object data, int cacheTime);

        Task<bool> IsSetAsync(string key);

        Task RemoveAsync(string key);

        Task RemoveByPrefixAsync(string prefix);

        Task ClearAsync();
    }
}