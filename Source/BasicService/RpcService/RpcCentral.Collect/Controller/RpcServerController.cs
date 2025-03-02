using RpcCentral.Common;
using RpcCentral.DAL;
using RpcCentral.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.TcpClient;
using WeDonekRpc.TcpClient.Config;
namespace RpcCentral.Collect.Controller
{
    public class RpcServerController : DataSyncClass
    {
        private TcpClient _RemoteClient = null;
        private volatile bool _ServerIsOnline = false;
        public RpcServerController (long serverId)
        {
            this.ServerId = serverId;
        }
        public long ServerId
        {
            get;
        }
        public RemoteServerModel Server
        {
            get;
            private set;
        }

        public string GroupTypeVal
        {
            get;
            private set;
        }
        public bool ServerIsOnline => this._ServerIsOnline;

        public ContainerDatum Container { get; private set; }

        protected override void SyncData ()
        {
            using (IocScope scope = UnityHelper.CreateTempScore())
            {
                this.Server = scope.Resolve<IRemoteServerDAL>().GetRemoteServer(this.ServerId);
                if (this.Server == null)
                {
                    throw new ErrorException("rpc.server.not.find[id=" + this.ServerId + "]");
                }
                this._ServerIsOnline = this.Server.IsOnline;
                if (this.Server.IsContainer)
                {
                    this.Container = scope.Resolve<IContainerDAL>().Get(this.Server.ContainerId);
                }
                this.GroupTypeVal = scope.Resolve<IServerGroupDAL>().GetTypeVal(this.Server.GroupId);
                if (!this.Server.ConIp.IsNull())
                {
                    SocketConfig.AddConArg(this.Server.ConIp, this.Server.ServerPort, this.Server.PublicKey, new string[]
                    {
                      "RpcCentral_"+this.Server.BindIndex
                    });
                    this._RemoteClient = new TcpClient(this.Server.ConIp, this.Server.ServerPort);
                }
            }
        }
        public void SetVerNum (int verNum, int oldVerNum)
        {
            if (this.Server.VerNum == oldVerNum)
            {
                this.Server.VerNum = verNum;
            }
        }
        public int SyncVerNum (int verNum)
        {
            if (verNum != this.Server.VerNum)
            {
                int ver = UnityHelper.Resolve<IRemoteServerDAL>().GetVerNum(this.ServerId);
                if (ver != this.Server.VerNum)
                {
                    this.Server.VerNum = ver;
                }
                return ver;
            }
            return verNum;
        }
        public bool ServerOnline (string conIp)
        {
            if (conIp != this.Server.ConIp)
            {
                this.Server.ConIp = conIp;
                SocketConfig.AddConArg(conIp, this.Server.ServerPort, this.Server.PublicKey);
                this._RemoteClient = new TcpClient(conIp, this.Server.ServerPort);
                UnityHelper.Resolve<IRemoteServerDAL>().SetConIp(this.ServerId, conIp);
            }
            if (!this._ServerIsOnline)
            {
                if (UnityHelper.Resolve<IRemoteServerDAL>().ServerOnline(this.ServerId, RpcContralConfig.ServerIndex))
                {
                    this.SendOnline();
                }
                this._ServerIsOnline = true;
                this.Server.BindIndex = RpcContralConfig.ServerIndex;
                return true;
            }
            return false;
        }
        private bool _ServerOnline (IocScope scope)
        {
            if (!this._ServerIsOnline)
            {
                if (scope.Resolve<IRemoteServerDAL>().ServerOnline(this.ServerId, RpcContralConfig.ServerIndex))
                {
                    this.SendOnline();
                }
                this._ServerIsOnline = true;
                this.Server.BindIndex = RpcContralConfig.ServerIndex;
                return true;
            }
            return false;
        }

        public bool ServerOnline (string conIp, string verNum)
        {
            int ver = verNum.GetVer();
            if (ver != this.Server.ApiVer)
            {
                this.Server.ApiVer = ver;
                UnityHelper.Resolve<IRemoteServerDAL>().SetApiVer(this.ServerId, ver);
            }
            return this.ServerOnline(conIp);
        }
        private void _ServerOffline (IocScope scope)
        {
            if (this._ServerIsOnline)
            {
                this._ServerIsOnline = false;
                if (this.Server.BindIndex == RpcContralConfig.ServerIndex && scope.Resolve<IRemoteServerDAL>().ServerOffline(this.ServerId, RpcContralConfig.ServerIndex))
                {
                    this.SendOffline();
                }
            }
        }
        private long _errorNum = 0;

        public void ChecklsOnline (IocScope scope)
        {
            if (this._RemoteClient == null)
            {
                return;
            }
            else if (this._ServerIsOnline == false)
            {
                long num = Interlocked.Increment(ref this._errorNum);
                if (num == long.MaxValue)
                {
                    _ = Interlocked.Exchange(ref this._errorNum, 0);
                }
                else if (num % 10 < 5)
                {
                    return;
                }
            }
            if (!this._RemoteClient.CheckIsUsable(out _))
            {
                this._ServerOffline(scope);
            }
            else if (this._ServerOnline(scope))
            {
                _ = Interlocked.Exchange(ref this._errorNum, 0);
            }
        }

    }
}
