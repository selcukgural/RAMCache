using NUnit.Framework;

namespace RAMCache.Test
{
    [TestFixture]
    public class RAMCache_Get
    {
        [TestCase(new int(),typeof(int),ExpectedResult = true)]
        public bool Get_If_Key_Exists(object key, object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            cache.AddOrUpdate(key, value);
            return cache.Get(key, out _);
        }

        [TestCase(new long(), ExpectedResult = false)]
        public bool Get_If_Key_Not_Exists(object key)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            return cache.Get(key, out _);
        }
    }
}
