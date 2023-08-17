namespace CachingTest
{
    public class StaticzeCachingProvider : Nice.ICachingProvider
    {
        public Task Clear()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Existed(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task<T?> Read<T>(string cacheKey) where T : class
        {
            throw new NotImplementedException();
        }

        public Task Remove(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task Write<T>(string cacheKey, T value) where T : class
        {
            throw new NotImplementedException();
        }

        public Task Write<T>(string cacheKey, T value, DateTime? expireTime = null) where T : class
        {
            throw new NotImplementedException();
        }

        public Task Write<T>(string cacheKey, T value, TimeSpan? expireTime = null) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
