using RpcSync.Model;

namespace RpcSync.DAL
{
    public interface ISysConfigDAL
    {
        SysConfig[] GetSysConfig (string type);
        ConfigItemToUpdateTime[] GetToUpdateTime ();
    }
}