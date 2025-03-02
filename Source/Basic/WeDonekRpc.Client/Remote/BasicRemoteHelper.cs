using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;

using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Remote
{
    /// <summary>
    /// 使用单一节点类型负载
    /// </summary>
    internal class BasicRemoteHelper : IRemoteNode
    {
        private readonly IBalanced _Balanced = null;
        private readonly BalancedConfig _Config = null;

        public BasicRemoteHelper (BalancedType type, ServerConfig[] servers)
        {
            this._Config = new BalancedConfig(type, servers);
            this._Balanced = BalancedCollect.GetBalanced(type, this._Config.ToConfig());
        }

        public bool DistributeNode<T> (IRemoteConfig config, T model, out IRemote server)
        {
            return this._Balanced.DistributeNode(config, out server);
        }

        public bool DistributeNode (IRemoteConfig config, out IRemote server)
        {
            return this._Balanced.DistributeNode(config, out server);
        }

        public IRemoteCursor DistributeNode<T> (IRemoteConfig config, T model)
        {
            return this._Balanced.GetAllNode();
        }
    }
}
