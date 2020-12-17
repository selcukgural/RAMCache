using NUnit.Framework;

namespace RAMCache.Test
{
    [TestFixture]
    public class RAMCacheInfo_
    {
        #region GetRAMCacheInfo

        [Test(ExpectedResult = true)]
        public bool RAMCacheInfo_Constructor()
        {
            var ramCacheInfo = new RAMCacheInfo();
            return ramCacheInfo.TotalCacheHitCount.Equals(0) && ramCacheInfo.TotalUsageMemory.Equals(0);
        }


        [Test(ExpectedResult = true)]
        public bool Get_RAM_Cache_Info_Check_Cache_Hit_Count()
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());

            object key = "Necati Ering";
            object value = "C & C++ Instructor, Trainer & Mentor";

            cache.Add(key, value);
            cache.Get(key, out _);

            key = "Kaan Aslan";
            value = "President at CSD";
            cache.Add(key, value);
            cache.Get(key, out _);

            return cache.GetRAMCacheInfo().TotalCacheHitCount.Equals(2);
        }


        [Test(ExpectedResult = true)]
        public bool Get_RAM_Cache_Info_Check_Total_Usage_Memory()
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            for (var i = 0; i < 5; ++i)
            {
                cache.Add(i, i.ToString());
            }
            return cache.GetRAMCacheInfo().TotalUsageMemory != 0;
        }

        #endregion
    }
}
