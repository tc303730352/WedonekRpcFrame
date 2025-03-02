using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Remote
{
    /// <summary>
    /// 空服务节点
    /// </summary>
    internal class NullRemoteNode : IRemoteNode
    {
        public bool DistributeNode<T> (IRemoteConfig config, T model, out IRemote server)
        {
            server = null;
            return false;
        }

        public bool DistributeNode (IRemoteConfig config, out IRemote server)
        {
            server = null;
            return false;
        }

        public IRemoteCursor DistributeNode<T> (IRemoteConfig config, T model)
        {
            return null;
        }
    }
}
