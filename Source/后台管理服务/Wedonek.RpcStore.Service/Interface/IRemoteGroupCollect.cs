using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Interface
{
        public interface IRemoteGroupCollect
        {
                void SetWeight(long id, int weight);
                bool CheckIsExists(long merId);
                void DropBind(long id);
                RemoteGroup[] GetServers(long merId);
                void SetBindGroup(long merId, long serverId);
                RemoteGroup[] GetServers(long rpcMerId, long[] serverId);
        }
}