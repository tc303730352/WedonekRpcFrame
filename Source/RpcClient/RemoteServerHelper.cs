using RpcClient.Collect;
using RpcClient.Interface;
using RpcClient.ServerGroup;

using RpcModel;

namespace RpcClient
{
        internal class RemoteServerHelper
        {
                private static readonly IRemoteGroup _LocalGroup = new LocalRemoteHelper();

                private static IRemoteGroup _GetRemoteGroup(IRemoteConfig config)
                {
                        if (config.RegionId == 0 && config.RpcMerId == RpcStateCollect.RpcMerId)
                        {
                                return _LocalGroup;
                        }
                        else
                        {
                                return new OtherRemoteHelper(config.RpcMerId, config.RegionId);
                        }
                }

                internal static bool FindServer<T>(string systemType, IRemoteConfig config, T model, out IRemote remote)
                {
                        IRemoteGroup group = _GetRemoteGroup(config);
                        return group.FindServer(systemType, config, model, out remote);
                }
        }
}
