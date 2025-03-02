using RpcCentral.Collect;
using RpcCentral.Collect.Controller;
using RpcCentral.Collect.Model;
using RpcCentral.Common;
using RpcCentral.Model;
using RpcCentral.Model.DB;
using RpcCentral.Service.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Lock;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Service
{
    internal class ContrainerLoginService : IContrainerLoginService
    {
        private readonly IRpcTokenCollect _TokenCollect;
        private readonly IRpcMerCollect _MerCollect;
        private readonly IContainerGroupCollect _ContainerGroup;
        private readonly IContainerCollect _Container;
        private readonly IRpcServerCollect _Server;
        private readonly IRpcServerStateCollect _ServerState;
        private readonly ISystemTypeCollect _SystemType;
        public ContrainerLoginService (IRpcTokenCollect token,
            ISystemTypeCollect systemType,
            IContainerGroupCollect containerGroup,
            IRpcMerCollect mer,
            IContainerCollect container,
            IRpcServerCollect server,
            IRpcServerStateCollect serverState)
        {
            this._ContainerGroup = containerGroup;
            this._Container = container;
            this._SystemType = systemType;
            this._ServerState = serverState;
            this._Server = server;
            this._MerCollect = mer;
            this._TokenCollect = token;
        }
        private RemoteServerLogin _LoginParam;
        private SystemTypeDatum _SysType;
        private ContrainerBasic _Cont;
        private RpcMer _Mer;
        private RpcServerController _Service;
        private BasicContainerGroup _ContGroup;
        private string _RemoteIp;
        private string _ApiVer;
        private void _InitContrainer ()
        {
            this._ContGroup = this._ContainerGroup.GroupLogin(this._LoginParam.ServerMac, this._LoginParam.Container.HostIp);
            this._Cont = this._Container.RegContainer(new ContrainerLogin
            {
                ContGroupId = this._ContGroup.Id,
                ContrainerId = this._LoginParam.Container.ContainerId,
                InternalIp = this._LoginParam.Container.LocalIp,
                InternalPort = this._LoginParam.Container.InsidePort.GetValueOrDefault(this._SysType.DefPort)
            });
        }
        private void _InitServer ()
        {
            long serverId = 0;
            using (DataLock local = LockFactory.ApplyLock("ContReg_" + this._Cont.Id))
            {
                if (local.GetLock())
                {
                    ServerCont server = this._Server.FindService(new ContainerGetArg
                    {
                        ContainerGroupId = this._ContGroup.Id,
                        HoldRpcMerId = this._Mer.Id,
                        ServerPort = this._Cont.InternalPort
                    });
                    if (server.Id == 0)
                    {
                        DateTime now = DateTime.Now;
                        int port = this._LoginParam.Container.HostPort.GetValueOrDefault(this._SysType.DefPort);
                        RemoteServerConfig add = new RemoteServerConfig
                        {
                            SystemType = this._SysType.Id,
                            ServerIp = this._RemoteIp,
                            ServiceState = RpcServiceState.待启用,
                            IsContainer = true,
                            GroupId = this._SysType.GroupId,
                            LastOffliceDate = now.Date,
                            ServiceType = this._SysType.ServiceType,
                            RegionId = this._ContGroup.RegionId,
                            HoldRpcMerId = this._Mer.Id,
                            AddTime = now,
                            ApiVer = this._ApiVer.GetVer(),
                            BindIndex = RpcContralConfig.ServerIndex,
                            ConfigPrower = 10,
                            ContainerId = this._Cont.Id,
                            PublicKey = Tools.NewGuid().ToString("N").Substring(Tools.GetRandom(0, 19), 12),
                            ServerPort = port,
                            ServerMac = Tools.GetRandomMac(),
                            ContainerGroupId = this._ContGroup.Id,
                            ConIp = this._RemoteIp,
                            ServerName = string.Concat("容器_", this._SysType.SystemName, "节点"),
                            RemoteIp = this._RemoteIp,
                            RemotePort = port
                        };
                        serverId = this._Server.AddContainer(add, this._LoginParam.SystemType);
                    }
                    else
                    {
                        if (server.ContainerId != this._Cont.Id)
                        {
                            this._Server.SetContainerId(server.Id, this._Cont.Id);
                        }
                        serverId = server.Id;
                    }
                    local.Exit();
                }
            }
            this._Service = this._Server.GetRpcServer(serverId);
        }
        public RpcServerLoginRes Login (RpcServerLogin login, string remoteIp)
        {
            this._LoginParam = login.ServerLogin;
            this._ApiVer = login.ApiVer;
            this._RemoteIp = remoteIp;
            RpcTokenCache token = this._TokenCollect.Get(login.AccessToken);
            this._Mer = this._MerCollect.GetMer(token.RpcMerId);
            this._SysType = this._SystemType.GetSystemType(this._LoginParam.SystemType);
            this._InitContrainer();
            this._InitServer();
            this._ServerState.SyncRunState(this._Service.ServerId, this._LoginParam.Process);
            if (this._Service.ServerOnline(remoteIp, login.ApiVer))
            {
                this._TokenCollect.SetConServerId(token, this._Service.ServerId);
            }
            RemoteServerModel server = this._Service.Server;
            return new RpcServerLoginRes
            {
                OAuthConfig = new RpcConfig
                {
                    AllowConIp = this._Mer.AllowServerIp
                },
                ServerConfig = new RpcServerConfig
                {
                    Name = server.ServerName,
                    ConfigPrower = server.ConfigPrower,
                    RegionId = server.RegionId,
                    ServerId = this._Service.ServerId,
                    ServerPort = server.ServerPort,
                    ServerIp = server.ServerIp,
                    ContainerGroup = this._ContGroup.Id,
                    RecoveryLimit = server.RecoveryLimit,
                    RecoveryTime = server.RecoveryTime,
                    GroupId = server.GroupId,
                    GroupTypeVal = this._Service.GroupTypeVal,
                    ServiceState = server.ServiceState,
                    SystemType = server.SystemType,
                    PublicKey = server.PublicKey,
                    VerNum = server.VerNum
                }
            };
        }
    }
}
