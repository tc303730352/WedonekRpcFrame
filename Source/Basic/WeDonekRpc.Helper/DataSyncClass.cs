using System;
using WeDonekRpc.Helper.Interface;
using WeDonekRpc.Helper.Lock;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.Helper
{
    public abstract class DataSyncClass : IDataSyncClass, IDisposable
    {
        ~DataSyncClass ()
        {
            this.Dispose();
        }
        public DataSyncClass ()
        {
            int time = HeartbeatTimeHelper.HeartbeatTime;
            this.AddTime = time;
            this.HeartbeatTime = time;
        }
        public int AddTime { get; }

        private volatile bool _IsInit = false;

        private readonly SyncLock _Lock = new SyncLock();
        public bool IsInit => this._IsInit;

        public int HeartbeatTime { get; set; } = 0;
        public string Error
        {
            get;
            private set;
        }
        public int ResetTime { get; private set; }


        public bool Init ()
        {
            if ( this._Lock.GetLock() )
            {
                try
                {
                    this.SyncData();
                    this._IsInit = true;
                    this.HeartbeatTime = HeartbeatTimeHelper.HeartbeatTime;
                    return this._IsInit;
                }
                catch ( Exception e )
                {
                    ErrorException ex = ErrorException.FormatError(e);
                    new LogInfo(ex, "DataSyncClass")
                    {
                        LogTitle = "数据同步错误!",
                        LogContent = this.GetType().FullName
                    }.Save();
                    this.Error = ex.ToString();
                    this._IsInit = false;
                    return false;
                }
                finally
                {
                    this._Lock.Exit();
                }
            }
            else
            {
                if ( !this._IsInit && this.Error.IsNull() )
                {
                    this.Error = "public.data.sync.overtime";
                }
                this.HeartbeatTime = HeartbeatTimeHelper.HeartbeatTime;
            }
            return true;
        }
        protected virtual void SyncData ()
        {
        }

        public virtual void ResetLock ()
        {
            if ( this._IsInit )
            {
                this.ResetTime = HeartbeatTimeHelper.HeartbeatTime;
                this._Lock.Reset();
            }
        }

        public virtual void Dispose ()
        {
            this._Lock?.Dispose();
        }
    }
}
