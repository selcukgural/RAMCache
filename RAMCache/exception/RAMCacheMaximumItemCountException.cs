namespace RAMCache.Exception
{
    public class RAMCacheMaximumItemCountException: System.Exception
    {
        public RAMCacheMaximumItemCountException()
        {

        }

        public RAMCacheMaximumItemCountException(string message) : base(message)
        {

        }
        public RAMCacheMaximumItemCountException(string message, System.Exception innerException) : base(message, innerException)
        {

        }
    }
}
