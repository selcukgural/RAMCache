using System;
using NUnit.Framework;
using RAMCache.Exception;

namespace RAMCache.Test
{
    [TestFixture]
    public class Exceptions
    {
        #region RAMCacheKeyNullException

        [Test(ExpectedResult = true)]
        public bool RAMCacheKeyNullException_Constructor()
        {
            var ramCacheKeyNullException = new RAMCacheKeyNullException();
            return 
                string.IsNullOrEmpty(ramCacheKeyNullException.HelpLink)   &&
                string.IsNullOrEmpty(ramCacheKeyNullException.Source)     &&
                string.IsNullOrEmpty(ramCacheKeyNullException.StackTrace) &&
                ramCacheKeyNullException.InnerException == null;
        }

        [TestCase("exception message" ,ExpectedResult = true)]
        public bool RAMCacheKeyNullException_Constructor_2(string message)
        {
            var ramCacheKeyNullException = new RAMCacheKeyNullException(message,new ArgumentNullException("args cannot be null"));
            return !string.IsNullOrEmpty(ramCacheKeyNullException.Message) && ramCacheKeyNullException.InnerException?.GetType() == typeof(ArgumentNullException);
        }

        #endregion

        #region RAMCacheMaximumItemCountException
        [Test(ExpectedResult = true)]
        public bool RAMCacheMaximumItemCountException_Constructor()
        {
            var ramCacheKeyNullException = new RAMCacheMaximumItemCountException();
            return
                string.IsNullOrEmpty(ramCacheKeyNullException.HelpLink) &&
                string.IsNullOrEmpty(ramCacheKeyNullException.Source) &&
                string.IsNullOrEmpty(ramCacheKeyNullException.StackTrace) &&
                ramCacheKeyNullException.InnerException == null;
        }

        [TestCase("exception message", ExpectedResult = true)]
        public bool RAMCacheMaximumItemCountException_Constructor_2(string message)
        {
            var ramCacheKeyNullException = new RAMCacheMaximumItemCountException(message, new ArgumentNullException("args cannot be null"));
            return !string.IsNullOrEmpty(ramCacheKeyNullException.Message) && ramCacheKeyNullException.InnerException?.GetType() == typeof(ArgumentNullException);
        }
        #endregion

    }
}
