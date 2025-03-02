using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace WeDonekRpc.Helper.Lock
{
    /// <summary>
    /// 同步锁工厂
    /// </summary>
    public class SyncLockFactory
    {
        private static readonly ConcurrentDictionary<string, DataSyncLock> _Locks = new ConcurrentDictionary<string, DataSyncLock>();
        private static readonly Timer _ClearLock = new Timer(_Clear, null, 10000, 10000);


        public static DataSyncLock ApplyLock (string key)
        {
            DataSyncLock tlock = _Locks.GetOrAdd(key, a => new DataSyncLock(a));
            tlock.AddUseNum();
            return tlock;
        }

        private static void _Clear (object state)
        {
            if (_Locks.IsEmpty)
            {
                return;
            }
            string[] keys = _Locks.Keys.ToArray();
            int time = HeartbeatTimeHelper.HeartbeatTime - 10;
            keys.ForEach(c =>
            {
                if (_Locks[c].IsNoUse(time))
                {
                    _ = _Locks.TryRemove(c, out _);
                }
            });
        }
    }
}
