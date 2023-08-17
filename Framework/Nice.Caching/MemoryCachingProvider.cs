namespace Nice.Caching;
internal class MemoryCachingProvider : ICachingProvider
{
    //todo 通过emit生成缓存数据，这样可以为每一个数据类型生成一个缓存数组，干掉装箱拆箱
    private readonly Dictionary<string, object> caches = new();

    public Task<T?> Read<T>(string cacheKey) where T : class
    {
        if (caches.ContainsKey(cacheKey))
        {
            var item = caches[cacheKey] as T;
            return Task.FromResult(item);
        }
        return Task.FromResult<T?>(null);
    }

    public Task Remove(string cacheKey)
    {
        if (caches.ContainsKey(cacheKey))
        {
            caches.Remove(cacheKey);
        }
        return Task.CompletedTask;
    }

    public Task Clear()
    {
        caches.Clear();
        return Task.CompletedTask;
    }

    public Task Write<T>(string cacheKey, T value) where T : class
    {
        if (caches.ContainsKey(cacheKey))
        {
            caches[cacheKey] = value;
        }
        else
        {
            caches.Add(cacheKey,value);
        }
        return Task.CompletedTask;
    }

    public Task Write<T>(string cacheKey, T value, DateTime? expireTime = null) where T : class
    {
        Write(cacheKey,value);
        return Task.CompletedTask;
    }

    public Task Write<T>(string cacheKey, T value, TimeSpan? expireTime = null) where T : class
    {
        Write(cacheKey, value);
        return Task.CompletedTask;
    }

    public Task<bool> Existed(string cacheKey)
    {
        var res = caches.ContainsKey(cacheKey);
        return Task.FromResult(res);
    }
}