using System.Threading;

using SocketTcpServer.Interface;

namespace SocketTcpServer.Model
{
        internal class SyntonySet
        {
                public SyntonySet(int timeOut)
                {
                        this._SyntonyType = SyntonyType.同步;
                        this._TimeOut = timeOut;
                        this._Sync = new AutoResetEvent(false);
                }
                public SyntonySet(Async async, object arg)
                {
                        this.Async = async;
                        this.Arg = arg;
                }
                private readonly int _TimeOut = 0;
                private SyntonyType _SyntonyType = SyntonyType.异步;

                /// <summary>
                /// 回调方式
                /// </summary>
                public SyntonyType SyntonyType
                {
                        get => this._SyntonyType;
                        set => this._SyntonyType = value;
                }

                /// <summary>
                /// 异步处理的方法
                /// </summary>
                public Async Async
                {
                        get;
                }
                /// <summary>
                /// 参数
                /// </summary>
                public object Arg
                {
                        get;
                }
                private volatile bool _IsComplate = false;

                private readonly AutoResetEvent _Sync = null;
                public bool SyncData()
                {
                        if (this._IsComplate)
                        {
                                return true;
                        }
                        return this._Sync.WaitOne(this._TimeOut, true);
                }

                public void DisSync()
                {
                        if (!this._IsComplate)
                        {
                                this._IsComplate = true;
                                this._Sync.Set();
                                this._Sync.Dispose();
                        }
                }
        }
}
