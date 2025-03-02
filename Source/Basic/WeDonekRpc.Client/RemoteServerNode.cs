using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.ServerGroup;

using WeDonekRpc.Model;

namespace WeDonekRpc.Client
{
    internal class RemoteServerNode
    {
        private static readonly IRemoteGroup _LocalGroup = new LocalRemotNode();
        private static readonly IService _Service;
        static RemoteServerNode ()
        {
            _Service = RpcService.Service;
        }
        private static IRemoteGroup _GetRemoteGroup (IRemoteConfig config)
        {
            if (!config.RegionId.HasValue && ( !config.RpcMerId.HasValue || config.RpcMerId.Value == RpcStateCollect.RpcMerId ))
            {
                return _LocalGroup;
            }
            else
            {
                return new RegionRemoteNode(config.RpcMerId, config.RegionId);
            }
        }

        internal static bool FindServer<T> (string systemType, IRemoteConfig config, T model, out IRemote remote)
        {
            IRemoteGroup group = _GetRemoteGroup(config);
            if (!group.FindServer(systemType, config, model, out remote))
            {
                _Service.NoServerErrorEvent(config, systemType, model);
                return false;
            }
            return true;
        }
        internal static IRemoteCursor FindAllServer<T> (string systemType, IRemoteConfig config, T model)
        {
            IRemoteGroup group = _GetRemoteGroup(config);
            return group.FindAllServer(systemType, config, model);
        }
    }
}
