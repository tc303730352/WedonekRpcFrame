using System;
using System.Threading;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.TcpClient.Enum;
using WeDonekRpc.TcpClient.Log;

namespace WeDonekRpc.TcpClient.Model
{
    internal class SyntonySet : System.IDisposable
    {
        public SyntonySet (int timeOut)
        {
            this._TimeOut = timeOut;
            this._Sync = new AutoResetEvent(false);
        }
        public SyntonySet (Async async, object arg)
        {
            this.Async = async;
            this.Arg = arg;
            this.SyntonyType = SyntonyType.异步;
        }
        private readonly int _TimeOut = 0;

        /// <summary>
        /// 回调方式
        /// </summary>
        public SyntonyType SyntonyType;

        /// <summary>
        /// 异步处理的方法
        /// </summary>
        public Async Async;
        /// <summary>
        /// 参数
        /// </summary>
        public object Arg;

        private volatile bool _IsComplate = false;

        private readonly AutoResetEvent _Sync = null;
        public bool SyncData ()
        {
            if (Config.SocketConfig.SpinWait != 0)
            {
                Thread.SpinWait(Config.SocketConfig.SpinWait);
            }
            if (this._IsComplate)
            {
                return true;
            }
            try
            {
                return this._Sync.WaitOne(this._TimeOut, true);
            }
            catch
            {
                if (this._IsComplate)
                {
                    return true;
                }
                return this.SyncData();
            }
        }

        public void DisSync ()
        {
            if (!this._IsComplate)
            {
                this._IsComplate = true;
                try
                {
                    _ = this._Sync.Set();
                }
                catch (Exception e)
                {
                    IoLogSystem.AddErrorLog(e, "同步类异常!");
                }
            }
        }

        public void Dispose ()
        {
            try
            {
                this._Sync.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
