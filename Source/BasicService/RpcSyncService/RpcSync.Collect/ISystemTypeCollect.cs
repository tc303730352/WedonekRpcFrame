using WeDonekRpc.Model;
using RpcSync.Model;

namespace RpcSync.Collect
{
    public interface ISystemTypeCollect
    {
        string GetName (string sysType);
        RpcServerType GetServerType (long systemTypeId);
        long GetSystemTypeId (string systemType);
        SystemType[] GetSystemTypes ();

        string[] GetSystemTypeVals ();
        Dictionary<long, string> GetSystemTypeVals (long[] ids);
    }
}