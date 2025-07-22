using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Application.Interfaces.infractucture.Utility.Cache
{
    public interface ICache<T> : IDisposable
    {
        IEnumerable<string> GetKeys();
        Task<IEnumerable<string>> GetKeysAsync();

        IEnumerable<T> GetValues();
        Task<IEnumerable<T>> GetValuesAsync();

        IReadOnlyDictionary<string, T> GetValuesAsMemory();

        void Add(string key, T value, TimeSpan ttl);
        Task AddAsync(string key, T value, TimeSpan ttl);

        T? Get(string key);
        Task<T?> GetAsync(string key);

        bool TryGet(string key, out T item);
        Task<bool> TryGetAsync(string key, out T item);

        bool Contains(string key);
        Task<bool> ContainsAsync(string key);

        bool RemoveIfExpired(string key);
        bool Remove(string key);
        Task<bool> RemoveAsync(string key);

        void Clear();
        Task ClearAsync();
    }

}
