using Microsoft.Extensions.Caching.Memory;

namespace UniSys.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);

        public CacheService(IMemoryCache cache) => _cache = cache;

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            if (_cache.TryGetValue(key, out T? cached))
                return cached!;

            var value = await factory();

            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(expiration ?? DefaultExpiration);

            _cache.Set(key, value, options);
            return value;
        }

        public void Remove(string key) => _cache.Remove(key);
    }
}
