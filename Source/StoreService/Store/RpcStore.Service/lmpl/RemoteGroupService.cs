using RpcManageClient;
using RpcStore.Collect;
using RpcStore.Model.ContainerGroup;
using RpcStore.Model.DB;
using RpcStore.Model.ServerConfig;
using RpcStore.Model.ServerGroup;
using RpcStore.RemoteModel.ContainerGroup.Model;
using RpcStore.RemoteModel.ServerBind.Model;
using RpcStore.RemoteModel.ServerType.Model;
using RpcStore.Service.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcStore.Service.lmpl
{
    internal class RemoteGroupService : IRemoteGroupService
    {
        private readonly IRemoteGroupCollect _Group;
        private readonly IServerCollect _Server;
        private readonly IServerTypeCollect _ServerType;
        private readonly IServerRegionCollect _Region;
        private readonly IServerGroupCollect _ServerGroup;
        private readonly IContainerGroupCollect _ContainerGroup;
        private readonly IRpcServerCollect _RpcServer;
        public RemoteGroupService (IRemoteGroupCollect group,
            IServerTypeCollect serverType,
            IServerCollect server,
            IContainerGroupCollect containerGroup,
            IRpcServerCollect rpcServer,
            IServerRegionCollect region,
            IServerGroupCollect serverGroup)
        {
            this._ContainerGroup = containerGroup;
            this._Region = region;
            this._RpcServer = rpcServer;
            this._ServerGroup = serverGroup;
            this._Server = server;
            this._ServerType = serverType;
            this._Group = group;
        }
        public void Delete (long id)
        {
            RemoteServerGroupModel group = this._Group.Get(id);
            this._Group.Delete(group);
            this._RpcServer.RefreshConfig(group.RpcMerId, group.SystemType);
        }
        public ContainerGroupItem[] GetBindContainerGroup (BindGetParam param)
        {
            long[] groupId = this._Group.GetContainerGroupId(param);
            if (groupId.IsNull())
            {
                return new ContainerGroupItem[0];
            }
            ContainerGroupDto[] dtos = this._ContainerGroup.Gets(groupId);
            return dtos.ConvertAll(a => new ContainerGroupItem
            {
                Id = a.Id,
                Name = a.Name
            });
        }
        public BindServerGroupType[] GetGroupAndType (BindGetParam param)
        {
            Dictionary<long, int> typeId = this._Group.GetNumBySystemType(param);
            if (typeId.Count == 0)
            {
                return new BindServerGroupType[0];
            }
            ServerType[] types = this._ServerType.Gets(typeId.Keys.ToArray());
            if (types.IsNull())
            {
                return new BindServerGroupType[0];
            }
            ServerGroupModel[] groups = this._ServerGroup.GetGroup(types.Distinct(c => c.GroupId));
            return groups.ConvertAll(c => new BindServerGroupType
            {
                Id = c.Id,
                GroupName = c.GroupName,
                ServerType = types.Convert(a => a.GroupId == c.Id, a => new BindServerServerType
                {
                    Id = a.Id,
                    ServerNum = typeId[a.Id],
                    TypeVal = a.TypeVal,
                    SystemName = a.SystemName

                })
            });
        }
        public long[] CheckIsBind (long rpcMerId, long[] serverId)
        {
            return this._Group.CheckIsBind(rpcMerId, serverId);
        }
        public BindServerItem[] GetServerItems (ServerBindQueryParam query)
        {
            return this._Group.GetServerItems(query);
        }
        public PagingResult<BindRemoteServer> Query (long rpcMerId, BindQueryParam query, IBasicPage paging)
        {
            BindServerGroup[] server = this._Group.Query(rpcMerId, query, paging, out int count);
            if (server.IsNull())
            {
                return new PagingResult<BindRemoteServer>(count);
            }
            Dictionary<int, string> regions = this._Region.GetNames(server.Distinct(a => a.RegionId));
            Dictionary<long, string> types = this._ServerType.GetNames(server.Distinct(a => a.SystemType));
            Dictionary<long, string> group = this._ServerGroup.GetGroupName(server.Distinct(a => a.GroupId));
            BindRemoteServer[] states = server.ConvertMap<BindServerGroup, BindRemoteServer>((a, b) =>
            {
                b.Region = regions.GetValueOrDefault(a.RegionId);
                b.SystemType = types.GetValueOrDefault(a.SystemType);
                b.ServerGroup = group.GetValueOrDefault(a.GroupId);
                return b;
            });
            return new PagingResult<BindRemoteServer>(states, count);
        }

        public void SetWeight (SaveWeight obj)
        {
            this._Group.SetWeight(obj.Weight);
            this._RpcServer.RefreshConfig(obj.RpcMerId, obj.SystemType);
        }
        public void SetBindGroup (BindServer set)
        {
            long[] serverId = this._Group.CheckIsBind(set.RpcMerId, set.ServerId);
            if (!serverId.IsNull())
            {
                set.ServerId = set.ServerId.Remove(serverId);
                if (set.ServerId.Length == 0)
                {
                    return;
                }
            }
            ServerConfigDatum[] servers = this._Server.Gets(set.ServerId);
            Dictionary<long, string> sysTypes = this._ServerType.GetSystemTypeVal(servers.Distinct(a => a.SystemType));
            RemoteServerGroup[] adds = servers.ConvertAll(c => new RemoteServerGroup
            {
                RegionId = c.RegionId,
                ServerId = c.Id,
                SystemType = c.SystemType,
                TypeVal = sysTypes.GetValueOrDefault(c.SystemType),
                ServiceType = c.ServiceType
            });
            this._Group.Adds(set.RpcMerId, adds);
            adds.ForEach(c =>
            {
                this._RpcServer.RefreshConfig(set.RpcMerId, c.SystemType);
            });
        }

        public ServerBindVer[] GetBindVer (long rpcMerId, bool? isHold)
        {
            long[] serverId = this._Group.GetServerId(rpcMerId, isHold);
            if (serverId.IsNull())
            {
                return null;
            }
            SystemTypeVerNum[] vers = this._Server.GetVerNums(serverId);
            ServerType[] types = this._ServerType.Gets(vers.ConvertAll(a => a.SystemTypeId));
            if (types.IsNull())
            {
                return null;
            }
            ServerGroupModel[] groups = this._ServerGroup.GetGroup(types.Distinct(c => c.GroupId));
            return groups.ConvertAll(c => new ServerBindVer
            {
                Id = c.Id,
                GroupName = c.GroupName,
                SystemType = types.Convert(a => a.GroupId == c.Id, a => new SystemTypeBindVer
                {
                    Id = a.Id,
                    TypeName = a.SystemName,
                    VerNum = vers.Find(c => c.SystemTypeId == a.Id, c => c.VerNum.FormatVerNum())
                })
            });
        }
    }
}
