using System.Threading;

using RpcModel;

using RpcService.Collect;
using RpcService.Model.DAL_Model;

using SocketTcpClient;
using SocketTcpClient.Config;

using RpcHelper;
namespace RpcService.Controller
{
        internal class RpcServerController : DataSyncClass
        {
                private string _ApiVer = null;
                private TcpClient _RemoteClient = null;
                private volatile bool _ServerIsOnline = false;
                public RpcServerController(long serverId)
                {
                        this.ServerId = serverId;
                }
                public long ServerId
                {
                        get;
                }
                public string RemoteIp
                {
                        get;
                        private set;
                }
                public string ConIp { get; private set; }
                public string ServerName
                {
                        get;
                        private set;
                }
                public string ServerIp
                {
                        get;
                        private set;
                }
                public int ServerPort { get; private set; }
                public long GroupId { get; private set; }
                public string TypeVal { get; private set; }
                public long SystemType { get; private set; }
                public string PublicKey { get; private set; }
                public int RegionId { get; private set; }

                public short ConfigPrower { get; private set; }

                public bool ServerIsOnline => this._ServerIsOnline;

                public int BindIndex { get; private set; }

                public RpcServiceState ServiceState { get; private set; }

                protected override bool SyncData()
                {
                        if (!RpcServerCollect.GetRemoteServer(this.ServerId, out RemoteServerModel server, out string error))
                        {
                                this.Error = error;
                                return false;
                        }
                        else if (server == null)
                        {
                                this.Error = "rpc.server.not.find";
                                return false;
                        }
                        else if (!RpcServerGroupCollect.GetGroupTypeVal(server.GroupId, out string typeVal))
                        {
                                this.Error = "rpc.server.group.not.find";
                                return false;
                        }
                        else
                        {
                                this.ConfigPrower = server.ConfigPrower;
                                this.RegionId = server.RegionId;
                                this._ApiVer = server.ApiVer;
                                this.ServiceState = server.ServiceState;
                                this.ConIp = server.ConIp;
                                this.ServerName = server.ServerName;
                                this.RemoteIp = server.RemoteIp;
                                this.TypeVal = typeVal;
                                this.ServerIp = server.ServerIp;
                                this.PublicKey = server.PublicKey;
                                this.ServerPort = server.ServerPort;
                                this.SystemType = server.SystemType;
                                this.BindIndex = server.BindIndex;
                                this.GroupId = server.GroupId;
                                if (!this.ConIp.IsNull())
                                {
                                        SocketConfig.AddConArg(this.ConIp, this.ServerPort, this.PublicKey);
                                        this._RemoteClient = new TcpClient(this.ConIp, this.ServerPort);
                                }
                                return true;
                        }
                }
                public bool ServerOnline(string conIp)
                {
                        if (conIp != this.ConIp)
                        {
                                this.ConIp = conIp;
                                SocketConfig.AddConArg(conIp, this.ServerPort, this.PublicKey);
                                this._RemoteClient = new TcpClient(conIp, this.ServerPort);
                                new DAL.RemoteServerDAL().SetConIp(this.ServerId, conIp);
                        }
                        if (!this._ServerIsOnline)
                        {
                                if (new DAL.RemoteServerDAL().ServerOnline(this.ServerId))
                                {
                                        Config.WebConfig.SendOnline(this);
                                }
                                this._ServerIsOnline = true;
                                this.BindIndex = Config.WebConfig.ServerIndex;
                                return true;
                        }
                        return false;
                }
                public bool ServerOnline(string conIp, string verNum)
                {
                        if (verNum != this._ApiVer)
                        {
                                if (verNum.IsNull())
                                {
                                        verNum = "0.0.0";
                                }
                                new DAL.RemoteServerDAL().SetServerApiVer(this.ServerId, verNum);
                        }
                        return this.ServerOnline(conIp);
                }
                private void _ServerOffline()
                {
                        if (this._ServerIsOnline)
                        {
                                this._ServerIsOnline = false;
                                if (this.BindIndex == Config.WebConfig.ServerIndex && new DAL.RemoteServerDAL().ServerOffline(this.ServerId))
                                {
                                        Config.WebConfig.SendOffline(this);
                                }
                        }
                }
                private long _errorNum = 0;
                public void ChecklsOnline()
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
                                        Interlocked.Exchange(ref this._errorNum, 0);
                                }
                                else if (num % 10 < 5)
                                {
                                        return;
                                }
                        }
                        if (!this._RemoteClient.CheckIsUsable(out _))
                        {
                                this._ServerOffline();
                        }
                        else if (this.ServerOnline(this.ConIp))
                        {
                                Interlocked.Exchange(ref this._errorNum, 0);
                        }
                }

        }
}
