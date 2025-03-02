using RpcManageClient;
using RpcManageClient.Model;
using RpcStore.Collect;
using RpcStore.Model.ContainerGroup;
using RpcStore.Model.DB;
using RpcStore.Model.ServerConfig;
using RpcStore.RemoteModel.ServerConfig.Model;
using RpcStore.Service.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcStore.Service.lmpl
{
    internal class ServerService : IServerService
    {
        private readonly IServerCollect _Server;
        private readonly IRpcServerCollect _RpcServer;
        private readonly IRemoteGroupCollect _RemoteGroup;
        private readonly IServerTypeCollect _ServerType;
        private readonly IRpcMerServerVerCollect _ServerVer;
        private readonly IContainerGroupCollect _ContainerGroup;
        private readonly IRpcMerCollect _RpcMer;
        private readonly IServerGroupCollect _Group;
        private readonly IServerRegionCollect _Region;
        private readonly IContainerCollect _Container;
        public ServerService (IServerCollect server,
            IServerTypeCollect serverType,
            IRemoteGroupCollect remoteGroup,
            IServerRegionCollect region,
            IRpcMerCollect rpcMer,
            IRpcMerServerVerCollect serverVer,
            IContainerGroupCollect containerGroup,
            IRpcServerCollect rpcServer,
            IContainerCollect container,
            IServerGroupCollect group)
        {
            this._Container = container;
            this._ServerVer = serverVer;
            this._ContainerGroup = containerGroup;
            this._RemoteGroup = remoteGroup;
            this._RpcMer = rpcMer;
            this._RpcServer = rpcServer;
            this._Region = region;
            this._Group = group;
            this._ServerType = serverType;
            this._Server = server;
        }

        public long Add (ServerConfigAdd add)
        {
            ServiceAddDatum datum = add.ConvertMap<ServerConfigAdd, ServiceAddDatum>();
            RemoteServerTypeModel type = this._ServerType.Get(add.SystemType);
            datum.GroupId = type.GroupId;
            datum.ServiceType = type.ServiceType;
            datum.SystemTypeVal = type.TypeVal;
            return this._Server.Add(datum);
        }

        public void Delete (long id)
        {
            RemoteServerConfigModel config = this._Server.Get(id);
            if (config.IsOnline || ( config.ServiceState != RpcServiceState.待启用 && config.ServiceState != RpcServiceState.停用 ))
            {
                throw new ErrorException("rpc.store.server.not.allow.drop");
            }
            this._Server.Delete(config);
            this._Refresh(config);
        }
        private void _Refresh (RemoteServerConfigModel config)
        {
            this._RpcServer.RefreshService(config.ConvertMap<RemoteServerConfigModel, ServiceDatum>());
        }
        public RemoteServerDatum Get (long id)
        {
            RemoteServerConfigModel config = this._Server.Get(id);
            long[] rpcMerId = this._RemoteGroup.GetRpcMerId(id);
            RemoteServerDatum datum = config.ConvertMap<RemoteServerConfigModel, RemoteServerDatum>((a, b) =>
            {
                if (a.HoldRpcMerId != 0)
                {
                    b.HoldRpcMer = this._RpcMer.GetName(a.HoldRpcMerId);
                }
                b.GroupName = this._Group.GetName(a.GroupId);
                if (!rpcMerId.IsNull())
                {
                    b.RpcMer = this._RpcMer.GetBasic(rpcMerId);
                }
                b.SystemTypeName = this._ServerType.GetName(a.SystemType);
                b.Region = this._Region.GetName(config.RegionId);
                return b;
            });
            if (datum.IsContainer)
            {
                ContainerGroupModel dto = this._ContainerGroup.Get(config.ContainerGroupId.Value);
                datum.ContainerName = dto.Name;
                datum.HostIp = dto.HostIp;
                datum.InternalAddr = this._Container.GetInternalAddr(config.ContainerId.Value);
            }
            return datum;
        }
        public void SetServiceState (long id, RpcServiceState state)
        {
            RemoteServerConfigModel config = this._Server.Get(id);
            if (state == RpcServiceState.正常)
            {
                if (config.ServerCode.IsNull())
                {
                    throw new ErrorException("rpc.store.server.code.is.null");
                }
            }
            if (this._Server.SetState(config, state))
            {
                this._Refresh(config);
            }
        }

        public PagingResult<RemoteServer> Query (ServerConfigQuery query, IBasicPage paging)
        {
            ServerConfigDatum[] servers = this._Server.Query(query, paging, out int count);
            if (servers.IsNull())
            {
                return new PagingResult<RemoteServer>();
            }
            ContainerGroupDto[] containerGroup = this._ContainerGroup.Gets(servers.Convert(c => c.IsContainer, c => c.ContainerGroupId.Value));
            long[] typeId = servers.Distinct(c => c.SystemType);
            long[] rpcMerId = servers.Distinct(c => c.HoldRpcMerId);
            Dictionary<long, int> vers = this._ServerVer.GetVers(rpcMerId, typeId);
            Dictionary<long, string> types = this._ServerType.GetNames(typeId);
            Dictionary<long, string> groups = this._Group.GetGroupName(servers.Distinct(c => c.GroupId));
            Dictionary<int, string> regions = this._Region.GetNames(servers.Distinct(c => c.RegionId));
            Dictionary<long, string> rpcMer = this._RpcMer.GetNames(rpcMerId);
            Dictionary<long, string> conIp = this._Container.GetInternalAddr(servers.Convert(c => c.IsContainer, c => c.ContainerId.Value));
            RemoteServer[] list = servers.ConvertMap<ServerConfigDatum, RemoteServer>((a, b) =>
            {
                b.SystemTypeName = types.GetValueOrDefault(a.SystemType);
                b.GroupName = groups.GetValueOrDefault(a.GroupId);
                b.Region = regions.GetValueOrDefault(a.RegionId);
                b.HoldRpcMer = rpcMer.GetValueOrDefault(a.HoldRpcMerId);
                if (vers.TryGetValue(a.SystemType, out int ver))
                {
                    b.IsLockVer = ver == a.VerNum;
                }
                if (a.IsContainer)
                {
                    ContainerGroupDto dto = containerGroup.Find(c => c.Id == a.ContainerGroupId.Value);
                    b.Container = new ContainerShow
                    {
                        ContainerGroupId = a.ContainerGroupId.Value,
                        ContainerType = dto.ContainerType,
                        HostIp = dto.HostIp,
                        Name = dto.Name
                    };
                    b.ContainerIp = conIp.GetValueOrDefault(a.ContainerId.Value);
                }
                return b;
            });
            return new PagingResult<RemoteServer>(list, count);
        }

        public void SetService (long id, ServerConfigSet set)
        {
            RemoteServerConfigModel config = this._Server.Get(id);
            if (config.IsOnline || config.ServiceState == RpcServiceState.正常)
            {
                throw new ErrorException("rpc.store.server.not.allow.drop");
            }
            ServerSetDatum datum = set.ConvertMap<ServerConfigSet, ServerSetDatum>();
            datum.SystemTypeVal = this._ServerType.GetTypeVal(config.SystemType);
            if (this._Server.Set(config, datum))
            {
                this._Refresh(config);
            }
        }

        public RemoteServerModel GetDatum (long id)
        {
            RemoteServerConfigModel config = this._Server.Get(id);
            RemoteServerModel model = config.ConvertMap<RemoteServerConfigModel, RemoteServerModel>();
            if (model.HoldRpcMerId != 0)
            {
                model.HoldRpcMer = this._RpcMer.GetName(model.HoldRpcMerId);
            }
            return model;
        }

        public ServerItem[] GetItems (ServerConfigQuery query)
        {
            return this._Server.GetItems(query);
        }

        public string GetName (long serverId)
        {
            return this._Server.GetName(serverId);
        }

        public Dictionary<long, string> GetNames (long[] serverId)
        {
            return this._Server.GetNames(serverId);
        }

        public void SetVerNum (long id, int verNum)
        {
            RemoteServerConfigModel config = this._Server.Get(id);
            if (this._Server.SetVerNum(config, verNum))
            {
                this._RpcServer.RefreshVerNum(config.Id, verNum, config.VerNum);
            }
        }
    }
}
