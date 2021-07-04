using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using JOB_MANAGER.CrossCuttingConsers;
using System.Runtime.Caching;
using System.Diagnostics;

namespace JOB_MANAGER.Helper
{
    public class MemoryCacheManager : ICacheManager
    {
        protected ObjectCache _cache;
        protected CacheItemPolicy _policy;
        protected int CacheDuration = 60;

        public static MemoryCacheManager instance;        

        public MemoryCacheManager()
        {
            Trace.WriteLine("Cache Başlatıldı");
            _cache = MemoryCache.Default;
            _policy = new CacheItemPolicy
            {
                Priority = CacheItemPriority.NotRemovable,
                RemovedCallback = RemoveCallback                
            };
        }

        private void RemoveCallback(CacheEntryRemovedArguments arguments)
        {
            Trace.WriteLine("Cache yıkıldı");
            Trace.WriteLine("Key"+arguments.CacheItem.Key);
            Trace.WriteLine("Key" + arguments.CacheItem.Value);
        }

        public void Add(string key, object data)
        {
            if (data == null)
                return;
            
            _cache.Set(new CacheItem(key, data), _policy);
        }

        public T Get<T>(string key)
        {
            return (T)_cache.Get(key);
        }

        public bool isExists(string key)
        {
            return _cache.Any(w => w.Key == key);
        }

        public void Remove(string key)
        {
            if(isExists(key))
                _cache.Remove(key);
        }

        public void Clear()
        {
            foreach (var item in _cache)
            {
                Remove(item.Key);
            }
        }
    }
}