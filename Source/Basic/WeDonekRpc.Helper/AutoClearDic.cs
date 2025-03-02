using System;
using System.Collections.Concurrent;
using System.Linq;
using WeDonekRpc.Helper.Interface;
namespace WeDonekRpc.Helper
{
    public class AutoClearDic
    {

        public static void AutoClear<TKey, TValue> (ConcurrentDictionary<TKey, TValue> dic, int overTime = 600, int expire = -1) where TValue : IDataSyncClass
        {
            AutoTask.Add(new AutoClearDic<TKey, TValue>(dic, Tools.GetRandom(60, 90), overTime, expire));
        }
        public static void AutoClear<TKey, TValue> (ConcurrentDictionary<TKey, TValue> dic, Action<TKey, TValue> action, int overTime = 600) where TValue : IDataSyncClass
        {
            AutoTask.Add(new AutoClearDic<TKey, TValue>(dic, Tools.GetRandom(60, 90), overTime, action));
        }
        public static void AutoClear<TKey, TValue> (ConcurrentDictionary<TKey, TValue> dic, Action<TKey, TValue> action, Func<TKey, TValue, bool> check, int overTime = 600) where TValue : IDataSyncClass
        {
            AutoTask.Add(new AutoClearDic<TKey, TValue>(dic, Tools.GetRandom(60, 90), overTime, check, action));
        }
        public static void AutoClear<TKey, TValue> (ConcurrentDictionary<TKey, TValue> dic, Func<TKey, TValue, bool> check, int overTime = 600) where TValue : IDataSyncClass
        {
            AutoTask.Add(new AutoClearDic<TKey, TValue>(dic, Tools.GetRandom(60, 90), overTime, check));
        }
    }
    internal class AutoClearDic<TKey, TValue> : IAutoTask where TValue : IDataSyncClass
    {
        private readonly ConcurrentDictionary<TKey, TValue> _Dic = null;


        private readonly int _Interval;
        private readonly Action<TKey, TValue> _Action = null;
        private readonly Func<TKey, TValue, bool> _Check = null;

        private readonly int _OverTime = 600;

        private readonly int _ExpireTime = -1;

        private volatile int _ExecTime = 0;
        public AutoClearDic (ConcurrentDictionary<TKey, TValue> dic, int interval, int overTime, Action<TKey, TValue> action) : this(dic, interval, overTime)
        {
            this._Action = action;
        }
        public AutoClearDic (ConcurrentDictionary<TKey, TValue> dic, int interval)
        {
            this._ExecTime = HeartbeatTimeHelper.HeartbeatTime + interval;
            this._Interval = interval;
            this._Dic = dic;
        }
        public AutoClearDic (ConcurrentDictionary<TKey, TValue> dic, int interval, int overTime) : this(dic, interval)
        {
            this._OverTime = overTime;
        }
        public AutoClearDic (ConcurrentDictionary<TKey, TValue> dic, int interval, int overTime, Func<TKey, TValue, bool> check) : this(dic, interval, overTime)
        {
            this._Check = check;
        }
        public AutoClearDic (ConcurrentDictionary<TKey, TValue> dic, int interval, int overTime, Func<TKey, TValue, bool> check, Action<TKey, TValue> action) : this(dic, interval, overTime, check)
        {
            this._Action = action;
        }
        public AutoClearDic (ConcurrentDictionary<TKey, TValue> dic, int interval, int overTime, int expire) : this(dic, interval, overTime)
        {
            this._ExpireTime = expire;
        }

        private void _Drop (TKey key)
        {
            if (this._Dic.TryRemove(key, out TValue val))
            {
                val.Dispose();
                this._Action?.Invoke(key, val);
            }
        }

        public void ExecuteTask ()
        {
            int time = HeartbeatTimeHelper.HeartbeatTime - this._OverTime;
            TKey[] keys;
            if (this._ExpireTime == -1)
            {
                keys = this._Dic.Where(a => a.Value.HeartbeatTime <= time).Select(a => a.Key).ToArray();
            }
            else
            {
                int expire = HeartbeatTimeHelper.HeartbeatTime - this._ExpireTime;
                keys = this._Dic.Where(a => a.Value.AddTime <= expire || ( a.Value.HeartbeatTime <= time )).Select(a => a.Key).ToArray();
            }
            if (keys.Length > 0)
            {
                keys.ForEach(a =>
                 {
                     if (this._Check != null)
                     {
                         if (!this._Dic.TryGetValue(a, out TValue val) || !this._Check(a, val))
                         {
                             return;
                         }
                     }
                     this._Drop(a);
                 });
            }
        }

        public bool IsExec (int now)
        {
            if (this._ExecTime > now)
            {
                return false;
            }
            this._ExecTime = HeartbeatTimeHelper.HeartbeatTime + this._Interval;
            return true;
        }
    }
}
