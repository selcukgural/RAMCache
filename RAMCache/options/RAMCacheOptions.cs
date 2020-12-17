using System;

namespace RAMCache.Options
{
    public class RAMCacheOptions
    {
        protected internal RAMCacheExpireOptions RamCacheExpireOptions;
        internal event EventHandler<RAMCacheExpireEventArgs> RAMCacheExpireEventHandler;
        public RAMCacheOptions()
        {
        }

        public RAMCacheOptions(RAMCacheExpireOptions ramCacheExpireOptions)
        {
            RamCacheExpireOptions = ramCacheExpireOptions;
            RamCacheExpireOptions.RAMCacheExpireEventHandler += RAMCacheExpireEventHandler;
        }
    }
}