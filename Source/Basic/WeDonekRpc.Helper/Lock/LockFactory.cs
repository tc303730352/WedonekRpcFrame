using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace WeDonekRpc.Helper.Lock
{
    /// <summary>
    /// 锁工厂
    /// </summary>
    public class LockFactory
    {
       
        private static ConcurrentDictionary<string, DataLock> _Locks = new ConcurrentDictionary<string, DataLock>();
        private static Timer _ClearLock = new Timer(_Clear, null, 10000, 10000);


        public static DataLock ApplyLock(string key)
        {
            DataLock t= _Locks.GetOrAdd(key, a=>new DataLock());
            t.AddUseNum();
            return t;
        }

        private static void _Clear(object state)
        {
            if (_Locks.IsEmpty)
            {
                return;
            }
            string[] keys = _Locks.Keys.ToArray();
            int time = HeartbeatTimeHelper.HeartbeatTime - 10;
            keys.ForEach(c => {
                if (_Locks[c].IsNoUse(time))
                {
                    _Locks.TryRemove(c, out _);
                }
            });
        }
    }
}
