using System;
using System.Threading;
using System.Threading.Tasks;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.Helper
{
    internal class EntrustRes
    {
        public bool isError;
        public string error;
    }
    public class AutoRetryEntrust
    {
        private readonly int _MaxRetryNum = 0;
        private Action _Action = null;
        private Action<string> _Error = null;
        private Timer _Timer = null;
        private volatile bool _IsCancel = false;
        private readonly int _DueTime = 0;
        private EntrustRes _SyncResult = null;
        private AutoResetEvent _Sync = null;
        private readonly int _TimeOut = 10000;//同步超时时间

        protected int RetryNum { get; private set; } = 0;

        public AutoRetryEntrust ( Action action, Action<string> error, int dueTime = 2000, int retryNum = 3 ) : this(dueTime, retryNum)
        {
            this._Error = error;
            this._Action = action;
        }
        protected AutoRetryEntrust ( int dueTime = 2000, int retryNum = 3 )
        {
            this._DueTime = dueTime;
            this._MaxRetryNum = retryNum;
        }
        public AutoRetryEntrust ( Action action, int dueTime = 2000, int retryNum = 3 ) : this(dueTime, retryNum)
        {
            this._Error = this._RetryError;
            this._Action = action;
        }
        protected internal void _Init ( Action action, Action<string> error )
        {
            this._Action = action;
            this._Error = error ?? this._RetryError;
        }
        public void ExecuteFun ()
        {
            if ( !this._ExecuteFun(out _) )
            {
                this._Timer = new Timer(new TimerCallback(this._RetryAction), null, this._DueTime, Timeout.Infinite);
            }
        }
        private bool _ExecuteFun ( out string error )
        {
            try
            {
                this._Action();
                error = null;
                return true;
            }
            catch ( Exception e )
            {
                ErrorException ex = ErrorException.FormatError(e);
                new ErrorLog(ex) { LogTitle = "委托重试执行失败!" }.Save();
                error = ex.ToString();
                return false;
            }
        }
        public Task Execute ()
        {
            return Task.Run(new Action(this.ExecuteFun));
        }
        public void SyncExecute ()
        {
            _ = Task.Run(new Action(this.ExecuteFun));
        }
        public bool ExecuteFun ( out string error )
        {
            Task<EntrustRes> res = Task.Run(() =>
            {
                if ( !this._ExecuteFun(out string msg) )
                {
                    this._Sync = new AutoResetEvent(false);
                    this._Timer = new Timer(new TimerCallback(this._RetryAction), null, this._DueTime, Timeout.Infinite);
                    return this._Sync.WaitOne(this._TimeOut, true)
                                        ? this._SyncResult
                                        : new EntrustRes
                                        {
                                            isError = true,
                                            error = msg
                                        };
                }
                return new EntrustRes();
            });
            error = res.Result.error;
            return !res.Result.isError;
        }
        private void _RetryError ( string error )
        {
            new WarnLog(error, "自动重试失败!")
                        {
                           { "RetryNum",this.RetryNum}
                        }.Save();
        }
        private void _SyncEnd ( string error )
        {
            if ( this._Sync != null )
            {
                this._SyncResult = new EntrustRes
                {
                    isError = true,
                    error = error
                };
                _ = this._Sync.Set();
                this._Sync.Dispose();
                this._Sync = null;
            }
        }
        private void _SyncEnd ()
        {
            if ( this._Sync != null )
            {
                this._SyncResult = new EntrustRes
                {
                    isError = false
                };
                _ = this._Sync.Set();
                this._Sync.Dispose();
                this._Sync = null;
            }
        }
        private void _RetryAction ( object state )
        {
            if ( this._ExecuteFun(out string error) )
            {
                this._SyncEnd();
                return;
            }
            ++this.RetryNum;
            if ( this.RetryNum <= this._MaxRetryNum )
            {
                this._Timer.Dispose();
                if ( this._IsCancel )
                {
                    return;
                }
                this._Timer = new Timer(this._RetryAction, null, this._DueTime, Timeout.Infinite);
            }
            else
            {
                this._Error?.Invoke(error);
                this._SyncEnd(error);
            }
        }

        public void Cancel ()
        {
            this._IsCancel = true;
            this._Timer.Dispose();
            this._SyncEnd("retry.entrust.cancel");
        }

        public static void ExecuteFun ( Action func )
        {
            new AutoRetryEntrust(func).ExecuteFun();
        }
        public static Task Execute ( Action func )
        {
            return new AutoRetryEntrust(func).Execute();
        }
        public static bool Execute ( Action func, out string error )
        {
            return new AutoRetryEntrust(func).ExecuteFun(out error);
        }

        public static void ExecuteFun<T> ( Action<T> func, T data )
        {
            new AutoRetryEntrust<T>(data, func).ExecuteFun();
        }
        public static Task Execute<T> ( Action<T> func, T data )
        {
            return new AutoRetryEntrust<T>(data, func).Execute();
        }
        public static bool Execute<T> ( Action<T> func, T data, out string error )
        {
            return new AutoRetryEntrust<T>(data, func).ExecuteFun(out error);
        }
    }
    public class AutoRetryEntrust<T> : AutoRetryEntrust
    {

        private readonly Action<T> _Action = null;
        private readonly Action<T, string> _Error = null;
        private readonly T _Param;

        public AutoRetryEntrust ( T datas, Action<T> action, Action<T, string> error, int dueTime = 2000, int retryNum = 3 ) : base(dueTime, retryNum)
        {
            this._Error = error;
            this._Param = datas;
            this._Action = action;
            this._Init(new Action(this._ExecuteFun), new Action<string>(this._RetryError));
        }
        public AutoRetryEntrust ( T datas, Action<T> action, int dueTime = 2000, int retryNum = 3 ) : base(dueTime, retryNum)
        {
            this._Param = datas;
            this._Action = action;
            this._Init(new Action(this._ExecuteFun), new Action<string>(this._RetryError));
        }

        private void _ExecuteFun ()
        {
            this._Action(this._Param);
        }

        private void _RetryError ( string error )
        {
            if ( this._Error != null )
            {
                this._Error(this._Param, error);
                return;
            }
            new WarnLog(error, "自动重试失败!")
                        {
                                { "Param",this._Param},
                                { "RetryNum",this.RetryNum}
                        }.Save();
        }
    }
}
