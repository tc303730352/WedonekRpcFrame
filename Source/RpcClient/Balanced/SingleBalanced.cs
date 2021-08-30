using RpcClient.Collect;
using RpcClient.Interface;

using RpcModel;

namespace RpcClient.Balanced
{
        /// <summary>
        /// 单例
        /// </summary>
        internal class SingleBalanced : IBalanced
        {
                public SingleBalanced(long serverId)
                {
                        this._ServerId = serverId;

                }
                private readonly long _ServerId = 0;

                public BalancedType BalancedType
                {
                        get;
                } = BalancedType.single;

                public bool DistributeNode(IRemoteConfig config, out IRemote server)
                {
                        if (this._ServerId != config.FilterServerId && RemoteServerCollect.GetRemoteServer(this._ServerId, out server) && server.IsUsable)
                        {
                                return true;
                        }
                        else
                        {
                                server = null;
                                return false;
                        }
                }
        }
}
