using RpcModel;
namespace RpcClient.Interface
{
        internal interface IRemoteGroup
        {
                bool FindServer(IRemoteConfig config, out IRemote server);
                bool FindServer<T>(string sysType, IRemoteConfig config, T model, out IRemote server);
        }
}