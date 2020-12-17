using NUnit.Framework;
using RAMCache.Exception;

namespace RAMCache.Test
{
    [TestFixture]
    public class RAMCache_Remove
    {
        [TestCase("Mevlüt", "Dinç", ExpectedResult = true)]
        public bool Remove_Exists_Entry(object key, object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            cache.Add(key, value);
            return cache.Remove(key, out var val) && val == value;
        }

        [TestCase("Özalp", "Babaoğlu", ExpectedResult = false)]
        public bool Remove_Not_Exists_Entry(object key, object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            return cache.Remove(key, out var val) && val == value;
        }
        [TestCase("", "Babaoğlu")]
        public void Remove_If_Key_Empty_Throws_RAMCacheKeyNullException(object key, object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            Assert.Throws<RAMCacheKeyNullException>(()=> cache.Remove(key, out _));
        }
        [TestCase(null, "Babaoğlu")]
        public void Remove_If_Null_Empty_Throws_RAMCacheKeyNullException(object key, object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            Assert.Throws<RAMCacheKeyNullException>(() => cache.Remove(key, out _));
        }
    }
}
