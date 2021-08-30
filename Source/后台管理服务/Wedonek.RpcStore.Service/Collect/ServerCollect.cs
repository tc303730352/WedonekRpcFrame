using System.Transactions;

using RpcClient;

using RpcManageClient;
using RpcManageClient.Model;

using RpcModel;
using RpcHelper;
using Wedonek.RpcStore.Service.DAL;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;
namespace Wedonek.RpcStore.Service.Collect
{

        internal class ServerCollect : BasicCollect<RemoteServerConfigDAL>, IServerCollect
        {
                private IRpcServerCollect _RpcServer => this.GetCollect<IRpcServerCollect>();
                public bool CheckIsExists(long sysTypeId)
                {
                        if (this.BasicDAL.CheckIsExists(sysTypeId, out bool isExists))
                        {
                                return isExists;
                        }
                        throw new ErrorException("rpc.server.check.error");
                }

                public void SetServiceState(long id, RpcServiceState state)
                {
                        RemoteServerConfig config = this.GetService(id);
                        if (config == null)
                        {
                                throw new ErrorException("rpc.server.not.find");
                        }
                        else if (!this.BasicDAL.SetServiceState(id, state))
                        {
                                throw new ErrorException("rpc.server.state.set.error");
                        }
                        config.ServiceState = state;
                        this._Refresh(config);
                }
                public long AddService(ServerConfigAddParam add)
                {
                        if (this.CheckServerPort(add.ServerMac, add.ServerPort))
                        {
                                throw new ErrorException("rpc.server.port.repeat");
                        }
                        add.ServerIndex = this._GetServiceIndex(add.SystemType, add.ServerMac);
                        if (!this.BasicDAL.AddService(add, out long id))
                        {
                                throw new ErrorException("rpc.server.add.error");
                        }
                        return id;
                }
                private int _GetServiceIndex(long sysTypeId, string mac)
                {
                        if (this.BasicDAL.GetServiceIndex(sysTypeId, mac, out int index))
                        {
                                return index;
                        }
                        throw new ErrorException("rpc.server.index.get.error");
                }
                public void SetService(long id, ServerConfigSetParam set)
                {
                        RemoteServerConfig config = this.GetService(id);
                        if (config.IsOnline)
                        {
                                throw new ErrorException("rpc.server.online");//服务节点只有在不在线的情况下更新信息
                        }
                        else if (set.ServerPort != config.ServerPort || set.ServerMac != config.ServerMac)
                        {
                                if (this.CheckServerPort(set.ServerMac, set.ServerPort))
                                {
                                        throw new ErrorException("rpc.server.port.repeat");
                                }
                        }
                        if (!this.BasicDAL.SetService(id, set))
                        {
                                throw new ErrorException("rpc.server.set.error");
                        }
                        config = config.ConvertInto(set);
                        this._Refresh(config);
                }
                private void _Refresh(RemoteServerConfig config)
                {
                        this._RpcServer.RefreshService(config.ConvertMap<RemoteServerConfig, ServiceDatum>());
                }
                public void DropService(long id)
                {
                        RemoteServerConfig config = this.GetService(id);
                        if (config.IsOnline)
                        {
                                throw new ErrorException("rpc.server.online");
                        }
                        using (TransactionScope tran = new TransactionScope())
                        {
                                if (!new DAL.RemoteServerGroupDAL().Clear(id))
                                {
                                        throw new ErrorException("rpc.server.group.clear.error");
                                }
                                else if (!this.BasicDAL.DropService(id))
                                {
                                        throw new ErrorException("rpc.server.drop.error");
                                }
                                else
                                {
                                        tran.Complete();
                                }
                        }
                        this._Refresh(config);
                }
                public ServerConfigDatum[] QueryService(QueryServiceParam query, IBasicPage paging, out long count)
                {
                        if (!this.BasicDAL.QueryService(query, paging, out ServerConfigDatum[] services, out count))
                        {
                                throw new ErrorException("rpc.server.query.error");
                        }
                        return services;
                }
                public ServerConfigDatum[] GetServices(long[] ids)
                {
                        if (ids.Length == 0)
                        {
                                return new ServerConfigDatum[0];
                        }
                        else if (!this.BasicDAL.GetServices(ids, out ServerConfigDatum[] configs))
                        {
                                throw new ErrorException("rpc.server.get.error");
                        }
                        else
                        {
                                return configs;
                        }
                }
                public RemoteServerConfig GetService(long id)
                {
                        if (!this.BasicDAL.GetService(id, out RemoteServerConfig config))
                        {
                                throw new ErrorException("rpc.server.get.error");
                        }
                        else if (config == null)
                        {
                                throw new ErrorException("rpc.server.not.find");
                        }
                        return config;
                }

                public bool CheckIsOnline(long id)
                {
                        if (this.BasicDAL.CheckIsOnline(id, out bool isOnline))
                        {
                                return isOnline;
                        }
                        throw new ErrorException("rpc.server.online.check.error");
                }

                public bool CheckServerPort(string mac, int serverPort)
                {
                        if (this.BasicDAL.CheckServerPort(mac, serverPort, out bool isExists))
                        {
                                return isExists;
                        }
                        throw new ErrorException("rpc.server.port.check.error");
                }

                public bool CheckIsExistsByGroup(long groupId)
                {
                        if (this.BasicDAL.CheckIsExistsByGroup(groupId, out bool isExists))
                        {
                                return isExists;
                        }
                        throw new ErrorException("rpc.server.check.error");
                }

                public bool CheckRegion(int regionId)
                {
                        if (this.BasicDAL.CheckRegion(regionId, out bool isExists))
                        {
                                return isExists;
                        }
                        throw new ErrorException("rpc.server.check.error");
                }
        }
}
