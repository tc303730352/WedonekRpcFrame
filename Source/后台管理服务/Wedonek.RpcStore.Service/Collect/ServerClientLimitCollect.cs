using RpcClient;

using RpcHelper;

using RpcManageClient;

using RpcModel.Model;

using Wedonek.RpcStore.Service.DAL;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Collect
{
        internal class ServerClientLimitCollect : BasicCollect<ServerClientLimitDAL>, IServerClientLimitCollect
        {
                private IRpcServerCollect _RpcServer => this.GetCollect<IRpcServerCollect>();
                public void Sync(ServerClientLimitData add)
                {
                        long limitId = this._FindLimitId(add.RpcMerId, add.ServerId);
                        if (limitId == 0)
                        {
                                this._Add(add);
                        }
                        else
                        {
                                this._Set(limitId, add);
                        }
                }
                private void _Add(ServerClientLimitData add)
                {
                        if (this.BasicDAL.AddConfig(add))
                        {
                                this._RpcServer.RefreshClientLimit(add.RpcMerId, add.ServerId);
                                return;
                        }
                        throw new ErrorException("rpc.client.limit.add.error");
                }
                private void _Set(long id, ServerClientLimitData config)
                {
                        if (this.BasicDAL.SetConfig(id, config.ConvertMap<ServerClientLimitData, ServerClientLimit>()))
                        {
                                this._RpcServer.RefreshClientLimit(config.RpcMerId, config.ServerId);
                                return;
                        }
                        throw new ErrorException("rpc.client.limit.set.error");
                }
                public void DropConfig(long rpcMerId, long serverId)
                {
                        long limitId = this._FindLimitId(rpcMerId, serverId);
                        if (limitId == 0)
                        {
                                return;
                        }
                        else if (!this.BasicDAL.DropConfig(limitId, out ServerClientLimitData config))
                        {
                                throw new ErrorException("rpc.limit.drop.error");
                        }
                        else
                        {
                                this._RpcServer.RefreshClientLimit(config.RpcMerId, config.ServerId);
                        }
                }
                public ServerClientLimitData GetConfig(long rpcMerId, long serverId)
                {
                        if (this.BasicDAL.GetConfig(rpcMerId, serverId, out ServerClientLimitData limit))
                        {
                                return limit;
                        }
                        throw new ErrorException("rpc.limit.get.error");
                }
                private long _FindLimitId(long rpcMerId, long serverId)
                {
                        if (this.BasicDAL.FindLimitId(rpcMerId, serverId, out long limitId))
                        {
                                return limitId;
                        }
                        throw new ErrorException("rpc.limit.find.error");
                }
        }
}
