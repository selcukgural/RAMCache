using System;

namespace RAMCache
{
    internal class RAMCacheExpireEventArgs : EventArgs
    {
        /// <summary>
        /// Entry key
        /// </summary>
        public object Key;
        /// <summary>
        /// Entry value
        /// </summary>
        public object Value;
        /// <summary>
        /// When the entry expired the entry after remove from cache this Action will be run
        /// </summary>
        public Action RunAfterExpire;
        /// <summary>
        /// When the entry expired the entry before remove from cache this Action will be run
        /// </summary>
        public Action RunBeforeExpire;
    }
}