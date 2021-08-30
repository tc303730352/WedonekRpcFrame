using RpcClient;

using RpcModel;

using RpcHelper;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Service
{
        internal class ServerService : IServerService
        {
                private readonly IServerCollect _Server = null;
                private readonly IServerTypeCollect _ServerType = null;
                private readonly IServerGroupCollect _Group = null;
                public ServerService(IServerCollect server, IServerTypeCollect serverType, IServerGroupCollect group)
                {
                        this._Group = group;
                        this._ServerType = serverType;
                        this._Server = server;
                }

                public long Add(ServerConfigAddParam add)
                {
                        return this._Server.AddService(add);
                }

                public void Drop(long id)
                {
                        this._Server.DropService(id);
                }

                public RemoteServerDatum Get(long id)
                {
                        RemoteServerConfig config = this._Server.GetService(id);
                        ServerGroup group = this._Group.GetGroup(config.GroupId);
                        ServerType type = this._ServerType.GetServiceType(config.SystemType);
                        return config.ConvertMap<RemoteServerConfig, RemoteServerDatum>((a, b) =>
                        {
                                b.GroupName = group.GroupName;
                                b.SystemName = type.SystemName;
                                b.BalancedType = type.BalancedType.ToString();
                                return b;
                        });
                }
                public void SetServiceState(long id, RpcServiceState state)
                {
                        this._Server.SetServiceState(id, state);
                }

                public RemoteServer[] Query(QueryServiceParam query, IBasicPage paging, out long count)
                {
                        ServerConfigDatum[] servers = this._Server.QueryService(query, paging, out count);
                        if (servers.Length == 0)
                        {
                                return new RemoteServer[0];
                        }
                        ServerType[] types = this._ServerType.GetServiceTypes(servers.ConvertAll(c => c.SystemType));
                        return servers.ConvertMap<ServerConfigDatum, RemoteServer>((a, b) =>
                        {
                                ServerType type = types.Find(c => c.Id == a.SystemType);
                                b.SystemName = type.SystemName;
                                b.BalancedType = type.BalancedType.ToString();
                                return b;
                        });
                }

                public void SetService(long id, ServerConfigSetParam set)
                {
                        this._Server.SetService(id, set);
                }
        }
}
