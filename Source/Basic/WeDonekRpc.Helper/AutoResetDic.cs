using System.Collections.Concurrent;
using System.Linq;
using WeDonekRpc.Helper.Interface;

namespace WeDonekRpc.Helper
{
    public class AutoResetDic
    {

        public static void AutoReset<TKey, TValue> (ConcurrentDictionary<TKey, TValue> dic, int resetTime = 180, int overTime = 300) where TValue : IDataSyncClass
        {
            AutoTask.Add(new AutoResetDic<TKey, TValue>(dic, Tools.GetRandom(60, 90), resetTime, overTime));
        }
    }
    internal class AutoResetDic<TKey, TValue> : IAutoTask where TValue : IDataSyncClass
    {
        private readonly ConcurrentDictionary<TKey, TValue> _Dic = null;


        private int _Interval { get; set; }

        private volatile int _ExecTime = 0;

        private readonly int _OverTime;

        private readonly int _ResetTime;


        public AutoResetDic (ConcurrentDictionary<TKey, TValue> dic, int interval, int resetTime, int overTime)
        {
            this._ExecTime = HeartbeatTimeHelper.HeartbeatTime + interval;
            this._ResetTime = resetTime;
            this._OverTime = overTime;
            this._Dic = dic;
            this._Interval = interval;
        }


        private void _Reset (TKey key)
        {
            if (this._Dic.TryGetValue(key, out TValue val))
            {
                val.ResetLock();
            }
        }

        public void ExecuteTask ()
        {
            int now = HeartbeatTimeHelper.HeartbeatTime;
            int time = now - this._ResetTime;
            int overTime = now - this._OverTime;
            TKey[] keys = this._Dic.Where(a => a.Value.HeartbeatTime <= time || a.Value.ResetTime <= overTime).Select(a => a.Key).ToArray();
            if (keys.Length > 0)
            {
                keys.ForEach(a =>
                {
                    this._Reset(a);
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
