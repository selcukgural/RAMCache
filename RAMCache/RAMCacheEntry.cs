using System;
using RAMCache.Options;

namespace RAMCache
{
    /// <summary>
    /// Internal Entry
    /// </summary>
    internal class RAMCacheEntry
    {
        /// <summary>
        /// Entry key
        /// </summary>
        public object Key;
        /// <summary>
        /// Entry value
        /// </summary>
        public object Value;
        /// <summary>
        /// When get this entry from cache then this value will be incremented
        /// </summary>
        public long CacheHitCount;

        internal event EventHandler<RAMCacheExpireEventArgs> RAMCacheExpireEventHandler;

        private RAMCacheExpireOptions _ramCacheExpireOptions;
        public RAMCacheExpireOptions RAMCacheExpireOptions
        {
            get => _ramCacheExpireOptions;
            set
            {
                _ramCacheExpireOptions = value;

                if(value == null) return;
                _ramCacheExpireOptions.RAMCacheExpireEventHandler += _ramCacheExpireOptions_RAMCacheExpireEventHandler;
            }
        }
        public RAMCacheEntry(object key, object value)
        {
            Key = key;
            Value = value;
        }
        private void _ramCacheExpireOptions_RAMCacheExpireEventHandler(object sender, RAMCacheExpireEventArgs e)
        {
            e.Key = Key;
            e.Value = Value;
            RAMCacheExpireEventHandler?.Invoke(sender, e);

            _ramCacheExpireOptions.RAMCacheExpireEventHandler -= _ramCacheExpireOptions_RAMCacheExpireEventHandler;
        }
    }
}