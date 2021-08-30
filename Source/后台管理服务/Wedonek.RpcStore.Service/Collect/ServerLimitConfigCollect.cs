using RpcClient;

using RpcManageClient;

using RpcModel.Model;

using RpcHelper;

using Wedonek.RpcStore.Service.DAL;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Collect
{
        internal class ServerLimitConfigCollect : BasicCollect<ServerLimitConfigDAL>, IServerLimitConfigCollect
        {
                private IRpcServerCollect _RpcServer => this.GetCollect<IRpcServerCollect>();
                public ServerLimitConfig GetLimitConfig(long serverId)
                {
                        if (!this.BasicDAL.GetConfig(serverId, out ServerLimitConfig config))
                        {
                                throw new ErrorException("rpc.limit.config.get.error");
                        }
                        return config;
                }
                public void DropConfig(long serverId)
                {
                        if (!this.BasicDAL.DropConfig(serverId))
                        {
                                throw new ErrorException("rpc.limit.config.drop.error");
                        }
                        this._RpcServer.RefreshLimit(serverId);
                }

                public void SyncConfig(AddServerLimitConfig add)
                {
                        ServerLimitConfig old = this.GetLimitConfig(add.ServerId);
                        if (old == null)
                        {
                                this._Add(add);
                                return;
                        }
                        else if (old.IsEquals<ServerLimitConfig>(add))
                        {
                                return;
                        }
                        this._Set(add);
                }
                private void _Set(AddServerLimitConfig config)
                {
                        ServerLimitConfig set = config.ConvertMap<AddServerLimitConfig, ServerLimitConfig>();
                        if (!this.BasicDAL.SetConfig(config.ServerId, set))
                        {
                                throw new ErrorException("rpc.limit.config.set.error");
                        }
                        this._RpcServer.RefreshLimit(config.ServerId);
                }
                private void _Add(AddServerLimitConfig add)
                {
                        if (!this.BasicDAL.AddConfig(add))
                        {
                                throw new ErrorException("rpc.limit.config.add.error");
                        }
                        this._RpcServer.RefreshLimit(add.ServerId);
                }
        }
}
