using RpcHelper;

namespace SocketTcpServer.Model
{
        internal class ServerConfig
        {
                private string _ServerKey = Tools.GetMD5("6xy3#7a%ad");

                /// <summary>
                /// 服务器的链接密钥
                /// </summary>
                public string ServerKey
                {
                        get => this._ServerKey;
                        set
                        {
                                if (string.IsNullOrEmpty(value))
                                {
                                        return;
                                }
                                this._ServerKey = Tools.GetMD5(value);
                        }
                }
                private Interface.ISocketEvent _SocketEvent = null;

                /// <summary>
                /// socket事件
                /// </summary>
                public Interface.ISocketEvent SocketEvent
                {
                        get => this._SocketEvent;
                        set => this._SocketEvent = value;
                }
                private Interface.IAllot _DefaultAllot = new SystemAllot.UserAllotInfo();

                /// <summary>
                /// 默认的处理程序
                /// </summary>
                public Interface.IAllot DefaultAllot
                {
                        get => this._DefaultAllot;
                        set => this._DefaultAllot = value;
                }

        }
}
