using HttpApiGateway.Model;

using RpcClient;

using RpcHelper;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Gateway.Model;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Service
{
        internal class ServerTypeService : IServerTypeService
        {
                private readonly IServerTypeCollect _ServerType = null;
                private IServerCollect _Server => RpcClient.RpcClient.Unity.Resolve<IServerCollect>();

                private IServerGroupCollect _ServerGroup => RpcClient.RpcClient.Unity.Resolve<IServerGroupCollect>();
                public ServerTypeService(IServerTypeCollect type)
                {
                        this._ServerType = type;
                }
                public long Add(ServerTypeDatum add)
                {
                        return this._ServerType.AddServiceType(add);
                }

                public bool CheckIsRepeat(string typeVal)
                {
                        return this._ServerType.CheckIsRepeat(typeVal);
                }

                public void Drop(long id)
                {
                        if (this._Server.CheckIsExists(id))
                        {
                                throw new ErrorException("rpc.server.type.noallow.drop");
                        }
                        this._ServerType.DropServiceType(id);
                }

                public ServerType Get(long id)
                {
                        return this._ServerType.GetServiceType(id);
                }
                public ServerType[] Gets(long groupId)
                {
                        return this._ServerType.GetServiceTypes(groupId);
                }

                public ServerTypeData[] Query(PagingParam<ServerTypeQueryParam> query, out long count)
                {
                        ServerType[] types = this._ServerType.QuerySystemType(query.Param, query.ToBasicPaging(), out count);
                        if (types.Length == 0)
                        {
                                return new ServerTypeData[0];
                        }
                        ServerGroup[] groups = this._ServerGroup.GetGroup(types.ConvertAll(a => a.GroupId));
                        return types.ConvertMap<ServerType, ServerTypeData>((a, b) =>
                        {
                                ServerGroup g = groups.Find(c => c.Id == a.GroupId);
                                b.GroupName = g.GroupName;
                                b.GroupVal = g.TypeVal;
                                return b;
                        });
                }

                public void Set(long id, ServerTypeSetParam param)
                {
                        this._ServerType.SetServiceType(id, param);
                }
        }
}
