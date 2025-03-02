using WeDonekRpc.Client.Interface;

using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Remote
{
    internal class BackUpRemoteServer : IRemoteNode
    {
        private readonly IRemoteNode _Server = null;
        private readonly IRemoteNode _BackUp = null;

        public BackUpRemoteServer (IRemoteNode main, IRemoteNode backUp)
        {
            this._Server = main;
            this._BackUp = backUp;
        }
        public bool DistributeNode<T> (IRemoteConfig config, T model, out IRemote server)
        {
            if (this._Server.DistributeNode(config, model, out server))
            {
                return true;
            }
            return this._BackUp.DistributeNode(config, model, out server);
        }

        public bool DistributeNode (IRemoteConfig config, out IRemote server)
        {
            if (this._Server.DistributeNode(config, out server))
            {
                return true;
            }
            return this._BackUp.DistributeNode(config, out server);
        }

        public IRemoteCursor DistributeNode<T> (IRemoteConfig config, T model)
        {
            return this._Server.DistributeNode<T>(config, model);
        }
    }
}
