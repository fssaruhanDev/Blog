using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Utilities.Cache.Item
{
    public class CacheItem
    {
        public CacheItem()
        {
            
        }

        public CacheItem(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public CacheItem(string key, string value, DateTime? expirationTime)
        {
            Key = key;
            Value = value;
            ExpirationTime = expirationTime;
        }

        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime? ExpirationTime { get; set; }


        public T getValue<T>() => (T)Convert.ChangeType(Value, typeof(T));
    }
}
