using RpcStore.Model.DB;
using RpcStore.Model.Group;
using RpcStore.Model.ServerGroup;
using RpcStore.RemoteModel.ServerBind.Model;
using WeDonekRpc.Model;

namespace RpcStore.DAL
{
    public interface IRemoteServerGroupDAL
    {
        long[] GetServerId (long rpcMerId, long sysTypeId);
        Dictionary<long, int> GetServerNum (long[] merId);
        bool CheckIsExists (long merId);
        bool CheckIsExists (long merId, long serverId);
        long[] GetIds (long serverId);
        long[] GetRpcMerId (long serverId);
        void Delete (long[] ids);
        void Delete (long id);
        RemoteServerGroupModel Get (long id);
        RemoteGroup[] GetServers (long merId);

        long[] GetServerIds (long rpcMerId, bool? isHold);
        void AddHold (long rpcMerId, RemoteServerGroup item);
        void Adds (long rpcMerId, RemoteServerGroup[] items);
        long[] CheckIsBind (long rpcMerId, long[] serverId);
        BindServerGroup[] Query (long rpcMerId, BindQueryParam query, IBasicPage paging, out int count);
        Dictionary<long, int> GetNumBySystemType (BindGetParam param);
        void SetWeight (Dictionary<long, int> weight);
        BindServerItem[] GetServerItems (ServerBindQueryParam query);

        RemoteServerGroupModel Find (long rpcMerId, long serverId);
        void SetIsHold (long id, bool isHold);
        long[] GetContainerGroupId (BindGetParam param);
        long[] GetServerIds (long rpcMerId, long systemTypeId);
        void Clear (long serverId);
    }
}