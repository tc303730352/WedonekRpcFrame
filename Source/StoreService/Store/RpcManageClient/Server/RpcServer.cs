using WeDonekRpc.Client.Server;
using WeDonekRpc.Helper;
using WeDonekRpc.Model.Model;
using WeDonekRpc.TcpClient;
using WeDonekRpc.TcpClient.Config;

namespace RpcManageClient.Server
{
    internal class RpcServer
    {
        public RpcServer (RpcToken token, RpcControlServer control)
        {
            this.ServerIp = control.ServerIp;
            this.ServerPort = control.Port;
            SocketConfig.AddConArg(this.ServerIp, this.ServerPort, null, token.AppId);
            this._RemoteClient = new TcpClient(this.ServerIp, this.ServerPort);
            this.IsUsable = true;
        }
        public bool IsUsable
        {
            get;
            private set;
        }
        public string ServerIp
        {
            get;
            set;
        }

        public int ServerPort
        {
            get;
            set;
        }
        private readonly TcpClient _RemoteClient = null;
        public bool CheckIsUsable ()
        {
            this.IsUsable = this._RemoteClient.CheckIsUsable(out _);
            return this.IsUsable;
        }
        public void Send<T> (string type, T data) where T : class
        {
            if (!this._RemoteClient.Send(type, data, out string error))
            {
                throw new ErrorException(error);
            }
        }
    }
}
