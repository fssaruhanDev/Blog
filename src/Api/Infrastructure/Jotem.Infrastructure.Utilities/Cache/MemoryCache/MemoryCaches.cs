using Jotem.Api.Application.Interfaces.infractucture.Utility.Cache;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jotem.Infrastructure.Utilities.Cache.MemoryCache
{
    public class MemoryCaches<T> : ICache<T>
    {
        private readonly IMemoryCache _cache;
        private readonly ConcurrentDictionary<string, DateTime> _expirationTimes;

        public MemoryCaches(IMemoryCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _expirationTimes = new ConcurrentDictionary<string, DateTime>();
        }

        public IEnumerable<string> GetKeys() => _expirationTimes.Keys;

        public Task<IEnumerable<string>> GetKeysAsync() => Task.FromResult(GetKeys());

        public IEnumerable<T> GetValues() => _expirationTimes.Keys.Select(key => _cache.TryGetValue(key, out T value) ? value : default).Where(v => v != null);

        public Task<IEnumerable<T>> GetValuesAsync() => Task.FromResult(GetValues());

        public IReadOnlyDictionary<string, T> GetValuesAsMemory() => _expirationTimes.Keys
            .Where(key => _cache.TryGetValue(key, out T value))
            .ToDictionary(key => key, key => _cache.Get<T>(key));

        public void Add(string key, T value, TimeSpan ttl)
        {
            _cache.Set(key, value, ttl);
            _expirationTimes[key] = DateTime.UtcNow.Add(ttl);
        }

        public Task AddAsync(string key, T value, TimeSpan ttl)
        {
            Add(key, value, ttl);
            return Task.CompletedTask;
        }

        public T Get(string key) => _cache.TryGetValue(key, out T value) ? value : default;

        public Task<T> GetAsync(string key) => Task.FromResult(Get(key));

        public bool TryGet(string key, out T item) => _cache.TryGetValue(key, out item);

        public Task<bool> TryGetAsync(string key, out T item)
        {
            var result = TryGet(key, out item);
            return Task.FromResult(result);
        }

        public bool Contains(string key) => _cache.TryGetValue(key, out _);

        public Task<bool> ContainsAsync(string key) => Task.FromResult(Contains(key));

        public bool Remove(string key)
        {
            _cache.Remove(key);
            return _expirationTimes.TryRemove(key, out _);
        }

        public Task<bool> RemoveAsync(string key)
        {
            var result = Remove(key);
            return Task.FromResult(result);
        }

        public void Clear()
        {
            foreach (var key in _expirationTimes.Keys)
                _cache.Remove(key);
            _expirationTimes.Clear();
        }

        public Task ClearAsync()
        {
            Clear();
            return Task.CompletedTask;
        }

        public bool IsExpire(string key)
        {
            return _expirationTimes.TryGetValue(key, out var expirationTime) && expirationTime <= DateTime.UtcNow;
        }

        public bool RemoveIfExpired(string key)
        {
            if (IsExpire(key)) return Remove(key);
            return false;
        }

        public void Dispose()
        {
            _cache.Dispose();
        }
    }
}
