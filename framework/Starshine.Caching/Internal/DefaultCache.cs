using FreeRedis;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Caching
{
    /// <summary>
    /// 内存缓存缓存
    /// </summary>
    internal class DefaultCache : ICache
    {
        private readonly IMemoryCache _memoryCache;

        public DefaultCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public CacheTypeEnum CacheType => CacheTypeEnum.Memory;

        public object Instance => _memoryCache;

        public object this[string key]
        {
            get => Get<object>(key);
            set => Set(key, value, -1);
        }

        public long Count => (_memoryCache as MemoryCache).Count;


        public bool ExistsKey(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            return _memoryCache.TryGetValue(key, out object _);
        }

        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            return _memoryCache.Get<T>(key);
        }


        public void Remove(params string[] keys)
        {
            foreach (var key in keys)
            {
                _memoryCache.Remove(key);
            }
        }

        public bool Set<T>(string key, T value, int expiry)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (expiry > 0)
            {
                var result = _memoryCache.Set(key, value, TimeSpan.FromSeconds(expiry));
                return result != null;

            }
            else
            {
                var result = _memoryCache.Set(key, value);
                return result != null;
            }
        }

        public bool Set<T>(string key, T value, TimeSpan? expiry = null)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (expiry.HasValue)
            {
                var result = _memoryCache.Set(key, value, expiry.Value);
                return result != null;
            }
            else
            {
                var result = _memoryCache.Set(key, value);
                return result != null;
            }
        }

        public string Get(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (ExistsKey(key))
            {
                var bytes = _memoryCache.Get<byte[]>(key);
                return Encoding.UTF8.GetString(bytes);
            }
            return null;
        }

        public bool Set(string key, string value, TimeSpan? expiry = null)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrEmpty(value))
            {
                return Set(key, Array.Empty<byte>(), expiry);
            }
            else
            {
                return Set(key, Encoding.UTF8.GetBytes(value), expiry);
            }
        }


        public IEnumerable<string> GetAllKeys()
        {
            var cacheType = _memoryCache.GetType();
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var coherentStateField = cacheType.GetField("_coherentState", flags);//增加一个获取CoherentState对象环节
            if (coherentStateField == null) return Enumerable.Empty<string>();
            var coherentState = coherentStateField.GetValue(_memoryCache);
            if (coherentState == null) return Enumerable.Empty<string>();
            var entriesField = coherentState.GetType().GetField("_entries", flags);
            if (entriesField == null) return Enumerable.Empty<string>();
            var cacheItems = entriesField.GetValue(coherentState) as IDictionary;
            if (cacheItems == null) return Enumerable.Empty<string>();
            return cacheItems.Keys.Cast<string>();
        }

        public long RemoveByPrefix(string prefixKey)
        {
            var allKeys = GetAllKeys();
            var keys = allKeys.Where(r => r.StartsWith(prefixKey));
            Remove(keys.ToArray());
            return keys.LongCount();
        }

        public void Clear()
        {
            var keys = GetAllKeys();
            Remove(keys.ToArray());
        }

        public KeyType TYPE(string key)
        {
            return KeyType.@string;
        }

        public void SetRedisDbNum(int dbNum)
        {
        }
    }
}
