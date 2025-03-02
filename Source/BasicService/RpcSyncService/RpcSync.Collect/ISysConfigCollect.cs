using RpcSync.Collect.Model;
using RpcSync.Model;

namespace RpcSync.Collect
{
    public interface ISysConfigCollect
    {
        Dictionary<string, SysConfigItem> GetSysConfig (string[] type);
        SysConfigItem GetSysConfig (string type);
        ConfigItemToUpdateTime[] GetToUpdateTime ();
        void Refresh (string type);
        void Refresh (string[] type);
    }
}