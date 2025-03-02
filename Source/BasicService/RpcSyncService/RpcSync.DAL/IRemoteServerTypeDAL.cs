using WeDonekRpc.Model;
using RpcSync.Model;

namespace RpcSync.DAL
{
    public interface IRemoteServerTypeDAL
    {
        string[] GetSystemTypeVals ();
        SystemType[] GetSystemType ();
        long GetSystemTypeId (string sysType);
        Dictionary<long, string> GetSystemTypeVals (long[] ids);
        RpcServerType GetServerType (long systemTypeId);
        string GetName (string sysType);
    }
}