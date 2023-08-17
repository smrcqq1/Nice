namespace Nice.Caching
{
    //todo ResponseCaching是前端的，所以可能不能这样封装
    internal class ResponseCachingProvider :MemoryCachingProvider, Nice.ICachingProvider
    {
    }
}