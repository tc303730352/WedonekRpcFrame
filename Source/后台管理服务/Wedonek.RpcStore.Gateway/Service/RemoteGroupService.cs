using HttpApiGateway.Model;

using RpcClient;

using RpcHelper;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Service
{
        internal class RemoteGroupService : IRemoteGroupService
        {
                private readonly IRemoteGroupCollect _Group = null;
                private readonly IServerCollect _Server = null;
                private readonly IServerTypeCollect _ServerType = null;
                private readonly IServerGroupCollect _ServerGroup = null;
                public RemoteGroupService(IRemoteGroupCollect group, IServerTypeCollect serverType, IServerCollect server, IServerGroupCollect serverGroup)
                {
                        this._ServerGroup = serverGroup;
                        this._Server = server;
                        this._ServerType = serverType;
                        this._Group = group;
                }
                public void Drop(long id)
                {
                        this._Group.DropBind(id);
                }

                public BindRemoteServer[] Get(long rpcMerId)
                {
                        RemoteGroup[] groups = this._Group.GetServers(rpcMerId);
                        if (groups.Length == 0)
                        {
                                return new BindRemoteServer[0];
                        }
                        ServerType[] types = this._ServerType.GetServiceTypes(groups.ConvertAll(a => a.SystemType));
                        ServerConfigDatum[] services = this._Server.GetServices(groups.ConvertAll(a => a.ServerId));
                        return groups.ConvertMap<RemoteGroup, BindRemoteServer>((a, b) =>
                        {
                                ServerConfigDatum server = services.Find(c => c.Id == a.ServerId);
                                b.IsOnline = server.IsOnline;
                                b.ServerId = server.Id;
                                b.ServerName = server.ServerName;
                                b.SystemName = types.Find(c => c.Id == a.SystemType, c => c.SystemName);

                                return b;
                        });
                }

                public RemoteServerBindState[] Query(long rpcMerId, PagingParam<QueryServiceParam> query, out long count)
                {
                        ServerConfigDatum[] services = this._Server.QueryService(query.Param, query.ToBasicPaging(), out count);
                        if (services.Length == 0)
                        {
                                return new RemoteServerBindState[0];
                        }
                        RemoteGroup[] server = this._Group.GetServers(rpcMerId, services.ConvertAll(a => a.Id));
                        ServerType[] types = this._ServerType.GetServiceTypes(services.ConvertAll(a => a.SystemType));
                        ServerGroup[] group = this._ServerGroup.GetGroup(services.ConvertAll(a => a.GroupId));
                        return services.ConvertMap<ServerConfigDatum, RemoteServerBindState>((a, b) =>
                        {
                                RemoteGroup k = server.Find(c => c.ServerId == a.Id);
                                b.IsBind = k != null;
                                b.BindId = k != null ? k.Id : 0;
                                ServerType type = types.Find(c => c.Id == a.SystemType);
                                b.SystemName = type.SystemName;
                                b.BalancedType = type.BalancedType.ToString();
                                b.GroupName = group.Find(c => c.Id == a.GroupId, c => c.GroupName);
                                return b;
                        });
                }
                public void SetWeight(long id, int weight)
                {
                        this._Group.SetWeight(id, weight);
                }
                public void SetBindGroup(BindServer set)
                {
                        this._Group.SetBindGroup(set.RpcMerId, set.ServerId);
                }
        }
}
