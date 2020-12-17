using System;
using System.Text;
using NUnit.Framework;

namespace RAMCache.Test
{
    [TestFixture]
    public class RAMCache_Clear
    {
        [Test(ExpectedResult = true)]
        public bool Clear_Dictionary()
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            cache.Add(14, new IntPtr(14));
            cache.Add(1, new ASCIIEncoding());

            cache.Clear();

            return cache.Count.Equals(0);
        }
    }
}
