using System;

namespace RpcHelper
{
        public class DataSyncClass : IDataSyncClass, IDisposable
        {
                ~DataSyncClass()
                {
                        this.Dispose();
                }
                public DataSyncClass() { }

                private volatile bool _IsInit = false;

                private readonly SyncLock _Lock = new SyncLock();
                public bool IsInit => this._IsInit;

                public int HeartbeatTime { get => this._HeartbeatTime; set => this._HeartbeatTime = value; }

                private int _HeartbeatTime = HeartbeatTimeHelper.HeartbeatTime;
                public string Error
                {
                        get;
                        protected set;
                }
                public bool Init()
                {
                        if (this._Lock.GetLock())
                        {
                                try
                                {
                                        this._IsInit = this.SyncData();
                                        this._HeartbeatTime = HeartbeatTimeHelper.HeartbeatTime;
                                        return this._IsInit;
                                }
                                catch (Exception e)
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
                                this._HeartbeatTime = HeartbeatTimeHelper.HeartbeatTime;
                        }
                        return true;
                }
                protected virtual bool SyncData()
                {
                        return true;
                }

                public virtual void ResetLock()
                {
                        if (this._IsInit)
                        {
                                this._Lock.Reset();
                                this._IsInit = false;
                        }
                }

                public virtual void Dispose()
                {
                        if (this._Lock != null)
                        {
                                this._Lock.Dispose();
                        }
                }
        }
}
