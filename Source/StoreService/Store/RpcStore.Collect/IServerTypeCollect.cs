using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ServerType.Model;

namespace RpcStore.Collect
{
    public interface IServerTypeCollect
    {
        string GetName (long id);

        string GetName (string typeVal);
        long Add (ServerTypeAdd datum);
        bool CheckIsExists (long groupId);
        void CheckIsRepeat (string typeVal);
        void Delete (RemoteServerTypeModel type);
        RemoteServerTypeModel Get (long id);
        long GetGroupId (long id);
        BasicServerType[] GetBasic (long[] ids);
        BasicServerType[] GetBasic (string[] types);
        Dictionary<long, string> GetNames (long[] ids);
        Dictionary<string, string> GetNames (string[] types);
        ServerType[] Gets (long groupId);
        ServerType[] Gets (long[] ids);
        ServerType[] Gets (string[] types);
        string GetTypeVal (long id);
        ServerType[] Query (ServerTypeQuery query, IBasicPage paging, out int count);
        bool Set (RemoteServerTypeModel type, ServerTypeSet param);
        ServerType[] GetAll ();
        Dictionary<long, string> GetSystemTypeVal (long[] typeId);
        long GetIdByTypeVal (string typeVal);
        Dictionary<long, int> GetNum(long[] longs);
    }
}