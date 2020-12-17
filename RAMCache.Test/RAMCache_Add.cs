using System;
using System.Diagnostics;
using NUnit.Framework;
using RAMCache.Exception;
using RAMCache.Options;

namespace RAMCache.Test
{
    [TestFixture]
    public class RAMCache_Add
    {
        #region RAMCache_Add

        [TestCase("Barış Manço", "Hal hal", ExpectedResult = true)]
        public bool Add_New_Entry_If_Success_Return_True(object key, object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            return cache.Add(key, value);
        }

        [TestCase("Barış Manço", "Hal hal", ExpectedResult = true)]
        public bool Add_New_Entry_With_ExpireTime_If_Success_Return_True(object key, object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            return cache.Add(key, value, TimeSpan.FromMinutes(2));
        }

        [TestCase("Barış Manço", "Hal hal", ExpectedResult = true)]
        public bool Add_New_Entry_With_RAMCacheExpireOptions_Only_ExpireTime_If_Success_Return_True(object key,
            object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            return cache.Add(key, value, new RAMCacheExpireOptions(TimeSpan.FromMinutes(1)));
        }

        [TestCase("Barış Manço", "Hal hal", ExpectedResult = true)]
        public bool Add_New_Entry_With_RAMCacheExpireOptions_Before_And_After_Actions_If_Success_Return_True(object key,
            object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            return cache.Add(key, value,
                new RAMCacheExpireOptions(TimeSpan.FromMinutes(1), () => Debug.WriteLine("After action"),
                    () => Debug.WriteLine("Before action")));
        }

        [TestCase("Barış Manço", "Dönence", ExpectedResult = false)]
        public bool Add_New_Entry_If_Key_Already_Exists_Return_False(object key, object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            return cache.Add(key, value) && cache.Add(key, value);
        }

        [TestCase(null, "Dağlar dağlar")]
        public void Add_New_Entry_If_Key_Is_Empty_Throws_RAMCacheKeyNullException(object key, object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            Assert.Throws<RAMCacheKeyNullException>(() => cache.Add(key, value));
        }

        [TestCase("", "Ahmet beyin ceketi")]
        public void Add_New_Entry_If_Key_Is_Null_Throws_RAMCacheKeyNullException(object key, object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            Assert.Throws<RAMCacheKeyNullException>(() => cache.Add(key, value));
        }

        [Test]
        public void Add_New_Entry_If_Maximum_Item_Count_Exceeded_Throws_RAMCacheMaximumItemCountException()
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions {MaximumItemCount = 2});
            cache.Add("Alan", "Turing");
            cache.Add("Mustafa", "Akgül"); //https://tr.wikipedia.org/wiki/Mustafa_Akgül
            Assert.Throws<RAMCacheMaximumItemCountException>(() => cache.Add("Dennis", "Ritchie"));
        }
        #endregion

        #region AddOrUpdate

        [TestCase("Barış Manço", "Hal hal")]
        public void Add_Or_Update_New_Entry_If_Success_Return_True(object key, object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            cache.AddOrUpdate(key, value);
        }

        [TestCase("Barış Manço", "Hal hal")]
        public void Add_Or_Update_New_Entry_With_ExpireTime_If_Success_Return_True(object key, object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            cache.AddOrUpdate(key, value, TimeSpan.FromMinutes(2));
        }

        [TestCase("Barış Manço", "Hal hal")]
        public void Add_Or_Update_New_Entry_With_RAMCacheExpireOptions_Only_ExpireTime_If_Success_Return_True(object key, object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            cache.AddOrUpdate(key, value, new RAMCacheExpireOptions(TimeSpan.FromMinutes(1)));
        }

        [TestCase("Barış Manço", "Hal hal")]
        public void Add_Or_Update_New_Entry_With_RAMCacheExpireOptions_Before_And_After_Actions(object key, object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            cache.AddOrUpdate(key, value, new RAMCacheExpireOptions(TimeSpan.FromMinutes(1), () => Debug.WriteLine("After action"), () => Debug.WriteLine("Before action")));

           //cache.RAMCache_Get()
        }

        [TestCase("Barış Manço", "Dönence")]
        public void Add_Or_Update_New_Entry_If_Key_Already_Exists(object key, object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            cache.AddOrUpdate(key, value);
            cache.AddOrUpdate(key, value);
        }

        [TestCase(null, "Dağlar dağlar")]
        public void Add_Or_Update_New_Entry_If_Key_Is_Empty_Throws_RAMCacheKeyNullException(object key, object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            Assert.Throws<RAMCacheKeyNullException>(() => cache.AddOrUpdate(key, value));
        }

        [TestCase("", "Ahmet beyin ceketi")]
        public void Add_Or_Update_New_Entry_If_Key_Is_Null_Throws_RAMCacheKeyNullException(object key, object value)
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions());
            Assert.Throws<RAMCacheKeyNullException>(() => cache.AddOrUpdate(key, value));
        }

        [Test]
        public void Add_Or_Update_New_Entry_If_Maximum_Item_Count_Exceeded_Throws_RAMCacheMaximumItemCountException()
        {
            var cache = new RAMCache(new Options.RAMCacheServiceOptions { MaximumItemCount = 2 });
            cache.AddOrUpdate("Alan", "Turing");
            cache.AddOrUpdate("Mustafa", "Akgül"); //https://tr.wikipedia.org/wiki/Mustafa_Akgül
            Assert.Throws<RAMCacheMaximumItemCountException>(() => cache.AddOrUpdate("Dennis", "Ritchie"));
        }
        #endregion
    }
}
