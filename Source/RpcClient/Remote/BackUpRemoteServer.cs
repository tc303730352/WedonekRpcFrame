using RpcClient.Interface;

using RpcModel;

namespace RpcClient.Remote
{
        internal class BackUpRemoteServer : IRemoteServer
        {
                private readonly IRemoteServer _Server = null;
                private readonly IRemoteServer _BackUp = null;

                public BackUpRemoteServer(IRemoteServer main, IRemoteServer backUp)
                {
                        this._Server = main;
                        this._BackUp = backUp;
                }
                public bool DistributeNode<T>(IRemoteConfig config, T model, out IRemote server)
                {
                        if (this._Server.DistributeNode(config, model, out server))
                        {
                                return true;
                        }
                        return this._BackUp.DistributeNode(config, model, out server);
                }

                public bool DistributeNode(IRemoteConfig config, out IRemote server)
                {
                        if (this._Server.DistributeNode(config, out server))
                        {
                                return true;
                        }
                        return this._BackUp.DistributeNode(config, out server);
                }
        }
}
