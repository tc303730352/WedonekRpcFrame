using RpcStore.Model.DB;
using RpcStore.Model.ServerConfig;
using RpcStore.RemoteModel.ServerConfig.Model;
using WeDonekRpc.Model;

namespace RpcStore.DAL
{
    public interface IRemoteServerConfigDAL
    {
        bool CheckServerCode (string serverCode, int ver);
        bool CheckServerName (string name);
        long Add (RemoteServerConfigModel add);
        bool CheckIsExists (long sysTypeId);
        bool CheckIsExistsByGroup (long groupId);
        bool CheckIsOnline (long id);
        bool CheckRegion (int regionId);
        bool CheckServerPort (string mac, int serverPort);
        void Delete (long id);
        RemoteServerConfigModel Get (long id);
        BasicService[] GetBasics (ServerConfigQuery query);
        Dictionary<long, string> GetNames (long[] ids);
        Dictionary<int, int> GetReginServerNum (int[] regionId);
        int? GetServiceIndex (long sysTypeId, string mac);
        ServerConfigDatum[] GetServices (long[] ids);
        ServerConfigDatum[] Query (ServerConfigQuery query, IBasicPage paging, out int count);
        void SetService (long id, ServerConfigSet data);
        void SetState (long id, RpcServiceState state);
        string GetName (long serverId);
        ServerItem[] GetItems (ServerConfigQuery query);
        Dictionary<long, int> GetContainerServerNum (long[] groupId, RpcServiceState[] states);
        Dictionary<long, int> GetServerNum (long[] typeId, RpcServiceState[] states);
        bool CheckIsExistByContainer (long containerGroupId);
        SystemTypeVerNum[] GetVerNums (long[] ids);
        SystemTypeVerNum[] GetAllVerNums (long[] ids);
        void SetVerNum (long id, int verNum);
        bool CheckIsExists (long systemType, string serverMac, int serverIndex);
        int GetVerNum (long rpcMerId, long systemTypeId);
    }
}