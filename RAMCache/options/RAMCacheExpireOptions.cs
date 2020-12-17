using System;
using System.Timers;

namespace RAMCache.Options
{
    public class RAMCacheExpireOptions
    {
        private TimeSpan _expireTime;
        public TimeSpan ExpireTime
        {
            get => _expireTime;
            set
            {
                _expireTime = value; 
                SetupTimer();
            }
        }

        public Action RunAfterExpire;
        public Action RunBeforeExpire;

        internal Timer Timer { get; private set; }

        internal event EventHandler<RAMCacheExpireEventArgs> RAMCacheExpireEventHandler;
        public RAMCacheExpireOptions(TimeSpan expireTime)
        {
            ExpireTime = expireTime;
        }
        public RAMCacheExpireOptions(TimeSpan expireTime, Action runAfterExpire, Action runBeforeExpire)
        {
            ExpireTime = expireTime;
            RunAfterExpire = runAfterExpire;
            RunBeforeExpire = runBeforeExpire;
        }
        private void InvokeTimerElapsed(object sender, ElapsedEventArgs e)
        {
            RAMCacheExpireEventHandler?.Invoke(sender,
                new RAMCacheExpireEventArgs {RunAfterExpire = RunAfterExpire, RunBeforeExpire = RunBeforeExpire});
        }

        private void SetupTimer()
        {
            Timer = new Timer { Enabled = true, Interval = ExpireTime.TotalMilliseconds };
            Timer.Elapsed += InvokeTimerElapsed;
        }
    }
}