using RpcStore.RemoteModel.SysEventConfig.Model;

namespace RpcStore.Collect
{
    public interface ISystemEventConfigCollect
    {
        SystemEventConfig Get (int id);
        string GetName (int sysEventId);
        Dictionary<int, string> GetName (int[] ids);
        SystemEventItem[] Gets ();
    }
}