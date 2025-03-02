using System;
using System.Threading;

namespace WeDonekRpc.Helper.Lock
{
    public class DataSyncLock : IDisposable
    {
        private int _UseNum = 0;
        private int _Time = int.MaxValue;
        private readonly SyncLock _Lock = new SyncLock();
        private volatile bool _IsSync = false;
        public string Key
        {
            get;
        }
        internal DataSyncLock (string key)
        {
            this.Key = key;
        }
        public object Result
        {
            get;
            private set;
        }

        public bool GetLock ()
        {
            if (this._Lock.GetLock())
            {
                this._IsSync = true;
                return true;
            }
            return false;
        }
        internal void AddUseNum ()
        {
            this._Time = HeartbeatTimeHelper.HeartbeatTime;
            _ = Interlocked.Add(ref this._UseNum, 1);
        }
        internal bool IsNoUse (int time)
        {
            return Interlocked.CompareExchange(ref this._UseNum, 0, 0) == 0 && this._Time < time;
        }

        public void Exit (object result)
        {
            if (this._IsSync)
            {
                this.Result = result;
                this._IsSync = false;
                this._Lock.Exit();
            }
        }
        public void Dispose ()
        {
            _ = Interlocked.Add(ref this._UseNum, -1);
            if (this._IsSync)
            {
                this._IsSync = false;
                this._Lock.Reset();
            }
        }
    }
}
