using NUnit.Framework;

namespace RAMCache.Test
{
    [TestFixture]
    public class RAMCacheServiceOptions
    {

        [Test(ExpectedResult = true)]
        public bool ServiceOptions_Not_Null()
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            return cache.ServiceOptions != null;
        }

        [Test(ExpectedResult = true)]
        public bool ServiceOptions_Maximum_Item_Count_Not_Null_nnn()
        {
            var cache = new RAMCache(null);
            return cache.ServiceOptions == null;
        }

        [TestCase(200,ExpectedResult = true)]
        public bool ServiceOptions_Maximum_Item_Count_Equals_Parameter(int maxCount)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions{MaximumItemCount = maxCount});
            return cache.ServiceOptions.MaximumItemCount.Equals(maxCount);
        }

        [TestCase(120,ExpectedResult = true)]
        public bool ServiceOptions_Not_Null_And_Maximum_Item_Count_Equals_Parameter(int maxCount)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions { MaximumItemCount = maxCount });
            return cache.ServiceOptions?.Value != null && cache.ServiceOptions.MaximumItemCount.Equals(maxCount);
        }
    }
}
