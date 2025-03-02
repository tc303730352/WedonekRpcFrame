using RpcStore.Model.DB;
using RpcStore.Model.Group;
using RpcStore.Model.ServerGroup;
using RpcStore.RemoteModel.ServerBind.Model;
using WeDonekRpc.Model;

namespace RpcStore.Collect
{
    public interface IRemoteGroupCollect
    {
        Dictionary<long, int> GetServerNum (long[] merId);
        bool CheckIsExists (long merId);
        void Delete (RemoteServerGroupModel group);
        RemoteServerGroupModel Get (long id);
        RemoteGroup[] GetServers (long merId);
        long[] CheckIsBind (long rpcMerId, long[] serverId);
        void Adds (long rpcMerId, RemoteServerGroup[] items);
        void SetWeight (Dictionary<long, int> weight);
        long[] GetRpcMerId (long serverId);

        BindServerGroup[] Query (long rpcMerId, BindQueryParam query, IBasicPage paging, out int count);
        Dictionary<long, int> GetNumBySystemType (BindGetParam param);
        BindServerItem[] GetServerItems (ServerBindQueryParam query);
        long[] GetContainerGroupId (BindGetParam param);
        long[] GetServerId (long rpcMerId, bool? isHold);
        long[] GetServerId (long rpcMerId, long sysTypeId);
    }
}