using RpcHelper;

using RpcManageClient;

using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Collect
{
        internal class RemoteGroupCollect : BasicCollect<DAL.RemoteServerGroupDAL>, IRemoteGroupCollect
        {
                private IRpcServerCollect _RpcServer => this.GetCollect<IRpcServerCollect>();
                public RemoteGroup[] GetServers(long merId)
                {
                        if (!this.BasicDAL.GetServers(merId, out RemoteGroup[] servers))
                        {
                                throw new ErrorException("rpc.remote.group.get.error");
                        }
                        return servers;
                }
                public bool CheckIsExists(long merId)
                {
                        if (!this.BasicDAL.CheckIsExists(merId, out bool isExists))
                        {
                                throw new ErrorException("rpc.remote.group.check.error");
                        }
                        return isExists;
                }
                private bool _CheckIsExists(long merId, long serverId)
                {
                        if (!this.BasicDAL.CheckIsExists(merId, serverId, out bool isExists))
                        {
                                throw new ErrorException("rpc.remote.group.check.error");
                        }
                        return isExists;
                }
                public void DropBind(long id)
                {
                        RemoteGroupAddParam data = this._Get(id);
                        if (data == null)
                        {
                                throw new ErrorException("rpc.remote.group.drop.error");
                        }
                        if (!this.BasicDAL.DropBind(id))
                        {
                                throw new ErrorException("rpc.remote.group.drop.error");
                        }
                        this._RpcServer.RefreshConfig(data.RpcMerId, data.SystemType);
                }
                private RemoteGroupAddParam _Get(long id)
                {
                        if (!this.BasicDAL.Get(id, out RemoteGroupAddParam datum))
                        {
                                throw new ErrorException("rpc.remote.group.get.error");
                        }
                        return datum;
                }
                public void SetBindGroup(long merId, long serverId)
                {
                        if (this._CheckIsExists(merId, serverId))
                        {
                                return;
                        }
                        ServerConfigDatum server = this.GetCollect<IServerCollect>().GetService(serverId);
                        ServerType type = this.GetCollect<IServerTypeCollect>().GetServiceType(server.SystemType);
                        RemoteGroupAddParam add = new RemoteGroupAddParam
                        {
                                RpcMerId = merId,
                                ServerId = serverId,
                                RegionId = server.RegionId,
                                SystemType = server.SystemType,
                                TypeVal = type.TypeVal
                        };
                        if (!this.BasicDAL.Add(add))
                        {
                                throw new ErrorException("rpc.remote.group.add.error");
                        }
                        this._RpcServer.RefreshConfig(merId, type.Id);
                }
                public void SetWeight(long id, int weight)
                {
                        if (!this.BasicDAL.SetWeight(id, weight))
                        {
                                throw new ErrorException("rpc.remote.group.set.error");
                        }
                }
                public RemoteGroup[] GetServers(long rpcMerId, long[] serverId)
                {
                        if (!this.BasicDAL.GetServers(rpcMerId, serverId, out RemoteGroup[] binds))
                        {
                                throw new ErrorException("rpc.remote.group.get.error");
                        }
                        return binds;
                }
        }
}
