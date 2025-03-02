using RpcSync.Collect;
using WeDonekRpc.Client.Broadcast;
using WeDonekRpc.Client.Interface;

namespace RpcSync.Service.Event
{
    internal class ServerGroupEvent : IRpcApiService
    {
        private readonly IRemoteServerGroupCollect _ServerGroup;

        public ServerGroupEvent (IRemoteServerGroupCollect serverGroup)
        {
            this._ServerGroup = serverGroup;
        }

        public void RefreshServerGroup (RefreshServerGroup refresh)
        {
            this._ServerGroup.Refresh(refresh.RpcMerId, refresh.RemoteId);
        }
    }
}
