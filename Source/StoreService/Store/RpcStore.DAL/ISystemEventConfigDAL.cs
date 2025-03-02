using RpcStore.RemoteModel.SysEventConfig.Model;

namespace RpcStore.DAL
{
    public interface ISystemEventConfigDAL
    {
        SystemEventConfig Get (int id);
        string GetName (int id);
        SystemEventItem[] Gets ();

        Dictionary<int, string> GetName (int[] ids);
    }
}