using SocketTcpServer.Enum;

using RpcHelper;

namespace SocketTcpServer.Model
{
        internal class PageDetailed
        {
                public PageDetailed(SyntonySet returnSet, Page page)
                {
                        this._Status = PageStatus.等待发送;
                        this._SyntonySet = returnSet;
                        this.PageType = page.PageType;
                        this.TimeOut = HeartbeatTimeHelper.HeartbeatTime + Config.SocketConfig.SyncTimeOut;
                }
                public PageType PageType
                {
                        get;
                        set;
                }
                private byte[] _ReturnData = null;

                internal byte[] ReturnData
                {
                        get => this._ReturnData;
                        set => this._ReturnData = value;
                }
                public Enum.SendType DataType
                {
                        get;
                        set;
                }
                public bool IsError
                {
                        get;
                        set;
                }
                /// <summary>
                /// 错误信息
                /// </summary>
                public string Error
                {
                        get;
                        set;
                }

                /// <summary>
                /// 最后更新时间
                /// </summary>
                public int TimeOut
                {
                        get;
                        set;
                }

                private volatile PageStatus _Status = PageStatus.等待发送;

                /// <summary>
                /// 包的状态
                /// </summary>
                internal PageStatus Status
                {
                        get => this._Status;
                        set => this._Status = value;
                }

                private SyntonySet _SyntonySet = null;

                /// <summary>
                /// 回调设置
                /// </summary>
                internal SyntonySet SyntonySet
                {
                        get => this._SyntonySet;
                        set => this._SyntonySet = value;
                }
        }
}
