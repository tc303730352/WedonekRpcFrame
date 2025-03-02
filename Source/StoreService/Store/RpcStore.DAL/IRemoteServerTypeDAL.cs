using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ServerType.Model;

namespace RpcStore.DAL
{
    public interface IRemoteServerTypeDAL
    {
        string GetName (string typeVal);
        string GetName (long id);
        string GetTypeVal (long id);
        long Add (RemoteServerTypeModel add);
        bool CheckIsExists (long groupId);
        bool CheckIsRepeat (string typeVal);
        void Delete (long[] ids);
        void Delete (long id);
        RemoteServerTypeModel Get (long id);
        BasicServerType[] GetBasic (long[] ids);
        BasicServerType[] GetBasic (string[] types);
        long[] GetIds (long groupId);
        ServerType[] Gets (long[] ids);
        ServerType[] Gets (string[] types);
        ServerType[] GetServiceTypes (long groupId);
        Dictionary<string, string> GetNames (string[] types);
        Dictionary<long, string> GetNames (long[] ids);
        ServerType[] Query (ServerTypeQuery query, IBasicPage paging, out int count);
        void Set (long id, ServerTypeSet param);
        ServerType[] GetAll ();
        long GetGroupId (long id);
        Dictionary<long, string> GetSystemTypeVal (long[] typeId);
        long GetIdByTypeVal (string typeVal);
        Dictionary<long, int> GetNum(long[] groupId);
    }
}