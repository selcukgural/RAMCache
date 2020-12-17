using Microsoft.Extensions.Options;

namespace RAMCache.Options
{
    public class RAMCacheServiceOptions : IOptions<RAMCacheServiceOptions>
    {
        public int? MaximumItemCount;
        public RAMCacheServiceOptions Value => this;
    }
}
