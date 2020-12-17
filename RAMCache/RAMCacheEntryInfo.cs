namespace RAMCache
{
    public class RAMCacheEntryInfo
    {
        public long CacheHitCount;
        //TODO
        //public long Size;

        public RAMCacheEntryInfo()
        {
            
        }

        public RAMCacheEntryInfo(long cacheHitCount)
        {
            CacheHitCount = cacheHitCount;
        }
    }
}
