namespace RAMCache
{
    public class RAMCacheInfo
    {
        public long TotalUsageMemory;
        public long TotalCacheHitCount;

        public RAMCacheInfo()
        {
            
        }

        public RAMCacheInfo(long totalUsageMemory, long totalCacheHitCount)
        {
            TotalUsageMemory = totalUsageMemory;
            TotalCacheHitCount = totalCacheHitCount;
        }
    }
}
