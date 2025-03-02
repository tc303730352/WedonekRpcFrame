using RpcCentral.Collect;
using RpcCentral.Collect.Controller;
using RpcCentral.Collect.Model;
using RpcCentral.Model;
using RpcCentral.Model.DB;
using RpcCentral.Service.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Service
{
    internal class ServerLoginService : IServerLoginService
    {
        private readonly IRpcTokenCollect _TokenCollect;
        private readonly IRpcMerCollect _MerCollect;
        private readonly IRpcServerCollect _Server;
        private readonly IRpcServerStateCollect _ServerState;
        private readonly ISystemTypeCollect _SystemType;
        public ServerLoginService (IRpcTokenCollect token,
            IRpcMerCollect mer,
            ISystemTypeCollect systemType,
            IRpcServerCollect server,
            IRpcServerStateCollect serverState)
        {
            this._SystemType = systemType;
            this._ServerState = serverState;
            this._Server = server;
            this._MerCollect = mer;
            this._TokenCollect = token;
        }

        public RpcServerLoginRes Login (RpcServerLogin login, string remoteIp)
        {
            RemoteServerLogin param = login.ServerLogin;
            long typeId = this._SystemType.GetSystemTypeId(param.SystemType);
            RpcServerController rpcServer = this._Server.FindRpcServer(typeId, param.ServerMac, param.ServerIndex);
            if (rpcServer.Server.ServiceState == RpcServiceState.停用)
            {
                throw new ErrorException("rpc.server.already.stop");
            }
            else if (rpcServer.Server.ServiceState == RpcServiceState.下线)
            {
                throw new ErrorException("rpc.server.already.offline");
            }
            RpcTokenCache token = this._TokenCollect.Get(login.AccessToken);
            if (rpcServer.Server.HoldRpcMerId != token.RpcMerId)
            {
                throw new ErrorException("rpc.server.hold.mer.error");
            }
            RpcMer mer = this._MerCollect.GetMer(token.RpcMerId);
            this._ServerState.SyncRunState(rpcServer.ServerId, param.Process);
            if (rpcServer.ServerOnline(remoteIp, login.ApiVer))
            {
                this._TokenCollect.SetConServerId(token, rpcServer.ServerId);
            }
            RemoteServerModel server = rpcServer.Server;
            return new RpcServerLoginRes
            {
                OAuthConfig = new RpcConfig
                {
                    AllowConIp = mer.AllowServerIp
                },
                ServerConfig = new RpcServerConfig
                {
                    Name = server.ServerName,
                    ConfigPrower = server.ConfigPrower,
                    RegionId = server.RegionId,
                    ServerId = rpcServer.ServerId,
                    ServerPort = server.ServerPort,
                    ServerIp = server.ServerIp,
                    RecoveryLimit = server.RecoveryLimit,
                    RecoveryTime = server.RecoveryTime,
                    GroupId = server.GroupId,
                    GroupTypeVal = rpcServer.GroupTypeVal,
                    ServiceState = server.ServiceState,
                    SystemType = server.SystemType,
                    PublicKey = server.PublicKey,
                    VerNum = server.VerNum
                }
            };
        }
    }
}
