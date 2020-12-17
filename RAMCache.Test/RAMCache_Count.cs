using NUnit.Framework;

namespace RAMCache.Test
{
    [TestFixture]
    public class RAMCache_Count
    {
        [TestCase(10, ExpectedResult = true)]
        public bool Count_Entry(int entryCount)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());

            for (var i = 0; i < entryCount; ++i)
            {
                cache.Add(i, i.ToString());
            }

            return cache.Count.Equals(entryCount);
        }
    }
}
