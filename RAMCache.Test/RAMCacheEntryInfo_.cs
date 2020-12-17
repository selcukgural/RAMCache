using NUnit.Framework;
using RAMCache.Exception;

namespace RAMCache.Test
{
    [TestFixture]
    public class RAMCacheEntryInfo_
    {
        #region GetRAMCacheEntryInfo

        [TestCase(2, ExpectedResult = true)]
        public bool RAMCacheEntryInfo_Constructor(long hitCount)
        {
            var ramCacheEntry = new RAMCacheEntryInfo(hitCount);
            return ramCacheEntry.CacheHitCount != 0 && ramCacheEntry.CacheHitCount.Equals(hitCount);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Get_RAM_Cache_Entry_Info_If_Key_Null_Or_Empty_Throws_RAMCacheKeyNullException(object key)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions { MaximumItemCount = 10 });
            Assert.Throws<RAMCacheKeyNullException>(() => cache.GetRAMCacheEntryInfo(key));
        }

        [TestCase("Ali Pınar", "https://www.linkedin.com/in/ali-pinar-6180834/", ExpectedResult = true)]
        public bool Get_RAM_Cache_Entry_Info_If_Key_Found(object key, object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            cache.Add(key, value);
            return cache.GetRAMCacheEntryInfo(key) != null;
        }

        [TestCase("Hakan  Hacıgümüş", "https://www.linkedin.com/in/hakanhacigumus/", ExpectedResult = true)]
        public bool Get_RAM_Cache_Entry_Info_If_Key_Not_Found(object key, object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            cache.Add(key, value);
            return cache.GetRAMCacheEntryInfo(key.ToString() + key) == null;
        }

        [TestCase("C ve Sistem Programcıları Derneği", "http://www.csystem.org/", ExpectedResult = true)]
        public bool Get_RAM_Cache_Entry_Info_Check_Cache_Hit_Count(object key, object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            cache.Add(key, value);
            cache.Get(key, out _);

            return cache.GetRAMCacheEntryInfo(key).CacheHitCount.Equals(1);
        }
        #endregion
    }
}
