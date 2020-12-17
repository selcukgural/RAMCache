using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RAMCache.Extension;

namespace RAMCache.Test
{
    [TestFixture]
    public class RAMCacheServiceCollectionExtensions
    {
        [TestCase(null)]
        public void AddRAMCache_If_Services_Null_Throws_ArgumentNullException(IServiceCollection services)
        {
            Assert.Throws<ArgumentNullException>(() => services.AddRAMCache());
        }

        [Test]
        public void AddRAMCache_If_Services_Null_And_RAMCacheServiceOptions_Not_Null()
        {
            ServiceCollection services = null;
            Assert.Throws<ArgumentNullException>(() => services.AddRAMCache(e => e.MaximumItemCount = 1));
        }

        [Test]
        public void AddRAMCache_If_Services_Not_Null_And_RAMCacheServiceOptions_Null()
        {
            ServiceCollection services = new ServiceCollection();
            Assert.Throws<ArgumentNullException>(() => services.AddRAMCache(null));
        }

        [Test(ExpectedResult = true)]
        public bool AddRAMCache_If_Services_Not_Null_And_RAMCacheServiceOptions_Not_Null()
        {
            ServiceCollection services = new ServiceCollection();
            const int maxItemCount = 100;
            services.AddRAMCache(e => e.MaximumItemCount = maxItemCount);

            RAMCache ramCache = (RAMCache)services.BuildServiceProvider().GetService(typeof(IRAMCache));

            return services.Any() && ramCache.ServiceOptions.MaximumItemCount == maxItemCount;
        }
        [Test(ExpectedResult = true)]
        public bool AddRAMCache_Add_Services_Successfuly()
        {
            var services = new ServiceCollection();
            services = (ServiceCollection)services.AddRAMCache();
            return services.Any();
        }
    }
}
