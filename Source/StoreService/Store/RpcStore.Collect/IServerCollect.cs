using RpcStore.Model.DB;
using RpcStore.Model.ServerConfig;
using RpcStore.RemoteModel.ServerConfig.Model;
using WeDonekRpc.Model;

namespace RpcStore.Collect
{
    public interface IServerCollect
    {
        long Add (ServiceAddDatum config);
        bool CheckIsExists (long sysTypeId);
        bool CheckIsExistsByGroup (long groupId);
        bool CheckIsOnline (long id);
        bool CheckRegion (int regionId);
        void CheckServerPort (string mac, int serverPort);
        void Delete (RemoteServerConfigModel config);
        RemoteServerConfigModel Get (long id);
        BasicService[] GetBasics (ServerConfigQuery query);
        ServerItem[] GetItems (ServerConfigQuery query);
        SystemTypeVerNum[] GetVerNums (long[] ids);
        SystemTypeVerNum[] GetAllVerNums (long[] ids);
        string GetName (long serverId);
        Dictionary<long, string> GetNames (long[] ids);
        Dictionary<int, int> GetReginServerNum (int[] regionId);
        ServerConfigDatum[] Gets (long[] ids);
        Dictionary<long, int> GetContainerServerNum (long[] groupId, RpcServiceState[] states);
        Dictionary<long, int> GetServerNum (long[] typeId, RpcServiceState[] states);
        ServerConfigDatum[] Query (ServerConfigQuery query, IBasicPage paging, out int count);
        bool Set (RemoteServerConfigModel config, ServerSetDatum set);
        bool SetState (RemoteServerConfigModel config, RpcServiceState state);
        bool CheckIsExistByContainer (long id);
        bool SetVerNum (RemoteServerConfigModel config, int verNum);
    }
}