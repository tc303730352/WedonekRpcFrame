using RpcClient.Helper;
using RpcClient.Interface;
using RpcClient.RpcApi;

using RpcModel;

using RpcHelper;
namespace RpcClient.Remote
{
        /// <summary>
        /// 节点信息
        /// </summary>
        internal class RemoteHelper : DataSyncClass, IRemoteServer
        {
                private IRemoteServer _Server = null;
                /// <summary>
                /// 下次重置节点配置事件
                /// </summary>
                private int _ResetTime = 0;

                /// <summary>
                /// 所属的系统组
                /// </summary>
                private readonly string _SystemType = null;
                /// <summary>
                /// 所属集群
                /// </summary>
                private readonly long _MerId = 0;
                private readonly int _RegionId = 0;
                public RemoteHelper(string sysType, long merId, int regionId) : this(sysType, merId)
                {
                        this._RegionId = regionId;
                }
                public RemoteHelper(string sysType, long merId)
                {
                        this._SystemType = sysType;
                        this._MerId = merId == 0 ? Collect.RpcStateCollect.RpcMerId : merId;
                        this._ResetTime = this.HeartbeatTime + Tools.GetRandom(60, 120);
                }
                protected override bool SyncData()
                {
                        if (!RpcServiceApi.GetRemoteServer(this._SystemType, this._MerId, this._RegionId, out GetServerListRes res, out string error))
                        {
                                this.Error = error;
                                return false;
                        }
                        else if (!res.Servers.IsNull())
                        {
                                IRemoteServer main = RpcClientHelper.GetRemoteServer(res.BalancedType, res.Servers);
                                if (!res.BackUp.IsNull())
                                {
                                        IRemoteServer back = RpcClientHelper.GetRemoteServer(res.BalancedType, res.BackUp);
                                        this._Server = new BackUpRemoteServer(main, back);
                                }
                                else
                                {
                                        this._Server = main;
                                }
                                return true;
                        }
                        this.Error = "rpc.server.no.config[" + this._SystemType + "," + this._MerId + "]";
                        return false;
                }

                /// <summary>
                /// 分配节点
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="config"></param>
                /// <param name="model"></param>
                /// <param name="server"></param>
                /// <returns></returns>
                public bool DistributeNode<T>(IRemoteConfig config, T model, out IRemote server)
                {
                        return this._Server.DistributeNode<T>(config, model, out server);
                }
                /// <summary>
                /// 分配节点
                /// </summary>
                /// <param name="config"></param>
                /// <param name="server"></param>
                /// <returns></returns>
                public bool DistributeNode(IRemoteConfig config, out IRemote server)
                {
                        return this._Server.DistributeNode(config, out server);
                }
                public void ResetLock(int time)
                {
                        if (time >= this._ResetTime)
                        {
                                this._ResetTime = time + Tools.GetRandom(60, 120);
                                base.ResetLock();
                        }
                }
        }
}