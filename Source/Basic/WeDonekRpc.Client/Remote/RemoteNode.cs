
using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.RpcApi;

using WeDonekRpc.Helper;

using WeDonekRpc.Model;
using WeDonekRpc.Model.Server;

namespace WeDonekRpc.Client.Remote
{
    /// <summary>
    /// 节点信息
    /// </summary>
    internal class RemoteNode : DataSyncClass, IRemoteNode
    {
        private IRemoteNode _Server = null;
        /// <summary>
        /// 下次重置节点配置事件
        /// </summary>
        private int _ResetTime = 0;


        private readonly RemoteBody _Body;
        private string _TransmitVer = null;
        private int _Ver = -1;
        internal RemoteNode ()
        {
            this._ResetTime = this.HeartbeatTime + Tools.GetRandom(60, 120);
        }
        public RemoteNode (string sysType, long merId, int? regionId) : this()
        {
            this._Body = new RemoteBody
            {
                rpcMerId = merId,
                regionId = regionId,
                systemType = sysType
            };
        }

        public RemoteNode (string sysType, long merId) : this()
        {
            this._Body = new RemoteBody
            {
                rpcMerId = merId,
                systemType = sysType
            };
        }
        protected override void SyncData ()
        {
            if (!RpcServiceApi.GetRemoteServer(this._Body, this._Ver, this._TransmitVer, out GetServerListRes res, out string error))
            {
                throw new ErrorException(error);
            }
            else if (this._TransmitVer != res.TransmitVer || this._Ver != res.ConfigVer)
            {
                this._Ver = res.ConfigVer;
                this._TransmitVer = res.TransmitVer;
                this._Server = this._GetServer(res);
            }
            else if (this._Server == null)
            {
                this._Server = new NullRemoteNode();
            }
        }
        private IRemoteNode _GetServer (GetServerListRes res)
        {
            if (res.Servers.IsNull())
            {
                this._ResetTime = HeartbeatTimeHelper.HeartbeatTime + Tools.GetRandom(5, 10);
                return new NullRemoteNode();
            }
            IRemoteNode main = RpcClientHelper.GetRemoteServer(res.BalancedType, res.Servers);
            if (!res.BackUp.IsNull())
            {
                IRemoteNode back = RpcClientHelper.GetRemoteServer(res.BalancedType, res.BackUp);
                return new BackUpRemoteServer(main, back);
            }
            else
            {
                return main;
            }
        }

        /// <summary>
        /// 分配节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <param name="model"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public bool DistributeNode<T> (IRemoteConfig config, T model, out IRemote server)
        {
            return this._Server.DistributeNode<T>(config, model, out server);
        }
        public IRemoteCursor DistributeNode<T> (IRemoteConfig config, T model)
        {
            return this._Server.DistributeNode<T>(config, model);
        }
        /// <summary>
        /// 分配节点
        /// </summary>
        /// <param name="config"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public bool DistributeNode (IRemoteConfig config, out IRemote server)
        {
            return this._Server.DistributeNode(config, out server);
        }
        public void ResetLock (int time)
        {
            if (time >= this._ResetTime)
            {
                this._ResetTime = time + Tools.GetRandom(60, 120);
                base.ResetLock();
            }
        }
    }
}