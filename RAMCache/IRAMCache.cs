using System;
using RAMCache.Options;

namespace RAMCache
{
    public interface IRAMCache
    {
        int Count { get; }
        RAMCacheEntryInfo GetRAMCacheEntryInfo(object key);
        RAMCacheInfo GetRAMCacheInfo();
        void Clear();
        bool Get(object key, out object value);
        bool Remove(object key, out object value);
        bool Add(object key, object value);
        void AddOrUpdate(object key, object value);
        bool Add(object key, object value, TimeSpan expireTime);
        void AddOrUpdate(object key, object value, TimeSpan expireTime);
        bool Add(object key, object value, RAMCacheExpireOptions options);
        void AddOrUpdate(object key, object value, RAMCacheExpireOptions options);
    }
}