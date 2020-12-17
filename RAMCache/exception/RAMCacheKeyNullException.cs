namespace RAMCache.Exception
{
    public class RAMCacheKeyNullException : System.Exception
    {
        public RAMCacheKeyNullException()
        {

        }

        public RAMCacheKeyNullException(string message) : base(message)
        {

        }
        public RAMCacheKeyNullException(string message, System.Exception innerException) : base(message, innerException)
        {

        }
    }
}