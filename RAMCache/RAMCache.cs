using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RAMCache.Exception;
using RAMCache.Options;

namespace RAMCache
{
    public class RAMCache : IRAMCache
    {
        /// <summary>
        /// Cached item count
        /// </summary>
        public int Count => _cache.Count;
        /// <summary>
        /// Options: Setting maximum entry count
        /// </summary>
        public readonly RAMCacheServiceOptions ServiceOptions;

        private readonly ConcurrentDictionary<object, RAMCacheEntry> _cache;
        private readonly object _lock = new object();

        public RAMCache(IOptions<RAMCacheServiceOptions> serviceOptions)
        {
            _cache = new ConcurrentDictionary<object, RAMCacheEntry>();
            ServiceOptions = serviceOptions?.Value;
        }

        #region Get
        /// <summary>
        /// Getting entry info
        /// </summary>
        /// <param name="key">cached item key</param>
        /// <returns>RAMCacheEntryInfo</returns>
        public RAMCacheEntryInfo GetRAMCacheEntryInfo(object key)
        {
            If_Key_Null_Then_Throw_Exception(key);

            if (!_cache.TryGetValue(key, out var entry)) return null;

            return new RAMCacheEntryInfo
            {
                CacheHitCount = Interlocked.Read(ref entry.CacheHitCount),
            };
        }
        /// <summary>
        /// Getting RAM cache info
        /// </summary>
        /// <returns>RAMCacheInfo</returns>
        public RAMCacheInfo GetRAMCacheInfo()
        {
            long totalMemory = GC.GetTotalMemory(true);
            long totalCacheHitCount = 0;

            Interlocked.Exchange(ref totalCacheHitCount, _cache.Select(e => e.Value).Sum(e => e.CacheHitCount));

            return new RAMCacheInfo(totalMemory, totalCacheHitCount);
        }
        /// <summary>
        /// Clear cached all the entries
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                _cache.Clear();
            }
        }
        /// <summary>
        /// The value corresponding to the key
        /// </summary>
        /// <param name="key">Entry key</param>
        /// <param name="value">Entry out value</param>
        /// <returns>if key exists <value>true</value> otherwise <value>false</value></returns>
        public bool Get(object key, out object value)
        {
            If_Key_Null_Then_Throw_Exception(key);

            value = null;
            if (!_cache.TryGetValue(key, out var ramCacheEntry)) return false;

            Interlocked.Increment(ref ramCacheEntry.CacheHitCount);
            Interlocked.Exchange(ref value, ramCacheEntry.Value);
            return true;
        }
        #endregion

        #region Remove
        /// <summary>
        /// Deletes the entry corresponding to the key
        /// </summary>
        /// <param name="key">Entry key</param>
        /// <param name="value">Entry out value</param>
        /// <returns>if the entry is deleted <value>true</value> otherwise <value>false</value></returns>
        public bool Remove(object key, out object value)
        {
            If_Key_Null_Then_Throw_Exception(key);

            value = null;
            if (!_cache.TryRemove(key, out var ramCacheEntry)) return false;

            Interlocked.Decrement(ref ramCacheEntry.CacheHitCount);
            Interlocked.Exchange(ref value, ramCacheEntry.Value);
            return true;
        }
        #endregion

        #region Add
        /// <summary>
        /// Add the entry corresponding to the key
        /// </summary>
        /// <param name="key">Entry key</param>
        /// <param name="value">Entry out value</param>
        /// <returns>if the entry is add <value>true</value> otherwise <value>false</value></returns>
        public bool Add(object key, object value)
        {
            If_Key_Null_Then_Throw_Exception(key);
            If_Maximum_Item_Count_Exceeded_Then_Throw_Exception();

            return _cache.TryAdd(key, new RAMCacheEntry(key, value));
        }
        /// <summary>
        /// If the key not exists it will be new entry otherwise it will be update exists the entry
        /// </summary>
        /// <param name="key">Entry key</param>
        /// <param name="value">Entry out value</param>
        public void AddOrUpdate(object key, object value)
        {
            If_Key_Null_Then_Throw_Exception(key);
            If_Maximum_Item_Count_Exceeded_Then_Throw_Exception();

            if (_cache.TryGetValue(key, out var ramCacheEntry))
            {
                ramCacheEntry.Value = value;
                _cache.AddOrUpdate(key, ramCacheEntry, (k, v) => ramCacheEntry);
            }
            else
            {
                _cache.TryAdd(key, new RAMCacheEntry(key, value));
            }
        }

        /// <summary>
        /// Add the entry corresponding to the key
        /// </summary>
        /// <param name="key">Entry key</param>
        /// <param name="value">Entry out value</param>
        /// <param name="expireTime">When will it expire</param>
        /// <returns>if the entry is add <value>true</value> otherwise <value>false</value></returns>
        public bool Add(object key, object value, TimeSpan expireTime)
        {
            If_Key_Null_Then_Throw_Exception(key);
            If_Maximum_Item_Count_Exceeded_Then_Throw_Exception();

            var entry = new RAMCacheEntry(key, value) {RAMCacheExpireOptions = new RAMCacheExpireOptions(expireTime)};
            entry.RAMCacheExpireEventHandler += RAMCacheExpireEventHandler;
            entry.RAMCacheExpireOptions.Timer.Start();

            return _cache.TryAdd(key, entry);
        }

        /// <summary>
        /// If the key not exists it will be new entry otherwise it will be update exists the entry
        /// </summary>
        /// <param name="key">Entry key</param>
        /// <param name="value">Entry out value</param>
        /// <param name="expireTime">When will it expire</param>
        public void AddOrUpdate(object key, object value, TimeSpan expireTime)
        {
            If_Key_Null_Then_Throw_Exception(key);
            If_Maximum_Item_Count_Exceeded_Then_Throw_Exception();

            if (_cache.TryGetValue(key, out var ramCacheEntry))
            {
                lock (_lock)
                {
                    ramCacheEntry.Value = value;

                    if (ramCacheEntry.RAMCacheExpireOptions == null)
                    {
                        ramCacheEntry.RAMCacheExpireOptions = new RAMCacheExpireOptions(expireTime);
                        ramCacheEntry.RAMCacheExpireOptions.Timer.Start();
                    }
                    else
                    {
                        ramCacheEntry.RAMCacheExpireOptions.ExpireTime = expireTime;
                        ramCacheEntry.RAMCacheExpireOptions.Timer.Start();
                    }
                }
                _cache.AddOrUpdate(key, ramCacheEntry, (k, v) => ramCacheEntry);
            }
            else
            {
                var entry = new RAMCacheEntry(key, value)
                {
                    RAMCacheExpireOptions = new RAMCacheExpireOptions(expireTime)
                };
                entry.RAMCacheExpireEventHandler += RAMCacheExpireEventHandler;
                entry.RAMCacheExpireOptions.Timer.Start();

                _cache.TryAdd(key, entry);
            }
        }

        /// <summary>
        /// Add the entry corresponding to the key
        /// </summary>
        /// <param name="key">Entry key</param>
        /// <param name="value">Entry out value</param>
        /// <param name="options">Expire options</param>
        /// <returns>if the entry is add <value>true</value> otherwise <value>false</value></returns>
        public bool Add(object key, object value, RAMCacheExpireOptions options)
        {
            If_Key_Null_Then_Throw_Exception(key);
            If_Maximum_Item_Count_Exceeded_Then_Throw_Exception();

            var entry = new RAMCacheEntry(key, value) {RAMCacheExpireOptions = options};
            entry.RAMCacheExpireEventHandler += RAMCacheExpireEventHandler;
            entry.RAMCacheExpireOptions.Timer.Start();

            return _cache.TryAdd(key, entry);
        }

        /// <summary>
        /// If the key not exists it will be new entry otherwise it will be update exists the entry
        /// </summary>
        /// <param name="key">Entry key</param>
        /// <param name="value">Entry out value</param>
        /// <param name="options">Expire options</param>
        public void AddOrUpdate(object key, object value, RAMCacheExpireOptions options)
        {
            If_Key_Null_Then_Throw_Exception(key);
            If_Maximum_Item_Count_Exceeded_Then_Throw_Exception();


            if (_cache.TryGetValue(key, out var ramCacheEntry))
            {
                lock (_lock)
                {
                    ramCacheEntry.Value = value;

                    ramCacheEntry.RAMCacheExpireOptions = options;
                    ramCacheEntry.RAMCacheExpireOptions.Timer.Start();
                }
                _cache.AddOrUpdate(key, ramCacheEntry, (k, v) => ramCacheEntry);
            }
            else
            {
                var entry = new RAMCacheEntry(key, value) {RAMCacheExpireOptions = options};
                entry.RAMCacheExpireEventHandler += RAMCacheExpireEventHandler;
                entry.RAMCacheExpireOptions.Timer.Start();

                _cache.TryAdd(key, entry);
            }
        }
        #endregion

        #region Private
        private async void RAMCacheExpireEventHandler(object sender, RAMCacheExpireEventArgs e)
        {
            try
            {
                if (!_cache.TryGetValue(e.Key, out var entry)) return;

                if (entry.RAMCacheExpireOptions.RunBeforeExpire != null)
                {
                    await Task.Factory.StartNew(entry.RAMCacheExpireOptions.RunBeforeExpire.Invoke);
                }

                //Remove entry
                _cache.TryRemove(e.Key, out _);

                if (entry.RAMCacheExpireOptions.RunAfterExpire != null)
                {
                    await Task.Factory.StartNew(entry.RAMCacheExpireOptions.RunAfterExpire.Invoke);
                }
            }
            catch
            {
                _cache.TryRemove(e.Key, out _);
                /*ignore*/
            }
        }
        private static void If_Key_Null_Then_Throw_Exception(object key)
        {
            if (key == null || string.IsNullOrEmpty(key.ToString()))
                throw new RAMCacheKeyNullException("key cannot be null");
        }
        private void If_Maximum_Item_Count_Exceeded_Then_Throw_Exception()
        {
            if (ServiceOptions.MaximumItemCount == null) return;

            lock (_lock)
            {
                if (_cache.Count < ServiceOptions.MaximumItemCount) return;
            }

            throw new RAMCacheMaximumItemCountException(
                $"Maximum number of items exceeded. {nameof(ServiceOptions.MaximumItemCount)}: {ServiceOptions.MaximumItemCount}");
        } 
        #endregion
    }
}
