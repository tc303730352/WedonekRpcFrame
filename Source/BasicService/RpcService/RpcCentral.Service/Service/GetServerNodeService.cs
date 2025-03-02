using RpcCentral.Collect;
using RpcCentral.Collect.Controller;
using RpcCentral.Common;
using RpcCentral.Model;
using RpcCentral.Service.Interface;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Service
{
    internal class GetServerNodeService : IGetServerNodeService
    {
        private readonly IRpcServerCollect _Server;
        private readonly IServerClientLimitCollect _ClientLimit;
        private readonly IReduceInRankCollect _ReduceInRank;
        public GetServerNodeService (IRpcServerCollect server,
            IReduceInRankCollect reduceInRank,
            IServerClientLimitCollect clientLimit)
        {
            this._ReduceInRank = reduceInRank;
            this._ClientLimit = clientLimit;
            this._Server = server;
        }
        public ServerConfigInfo Get (long serverId, long sourceId)
        {
            RpcServerController rpcServer = this._Server.GetRpcServer(serverId);
            RpcServerController source = this._Server.GetRpcServer(sourceId);
            RemoteServerModel sourServer = source.Server;
            RemoteServerModel datum = rpcServer.Server;
            ServerConfigInfo server = datum.ConvertMap<RemoteServerModel, ServerConfigInfo>();
            server.Name = datum.ServerName;
            server.ServerId = rpcServer.ServerId;
            server.Reduce = this._ReduceInRank.GetReduceInRank(datum.HoldRpcMerId, rpcServer.ServerId);
            server.ClientLimit = this._ClientLimit.GetClientLimit(sourServer.HoldRpcMerId, rpcServer.ServerId);
            if (datum.RegionId != sourServer.RegionId)
            {
                server.ServerIp = datum.RemoteIp;
                server.ServerPort = datum.RemotePort;
            }
            else if (datum.IsContainer && sourServer.IsContainer && datum.ContainerGroupId == sourServer.ContainerGroupId)
            {
                server.ServerIp = rpcServer.Container.InternalIp;
                server.ServerPort = rpcServer.Container.InternalPort;
            }
            else if (!datum.IsContainer && sourServer.IsContainer && source.Container.HostMac == datum.ServerMac)
            {
                server.ServerIp = source.Container.HostIp;
            }
            server.GroupTypeVal = rpcServer.GroupTypeVal;
            return server;
        }

    }
}
