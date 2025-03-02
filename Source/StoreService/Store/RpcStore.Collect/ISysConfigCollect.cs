using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.Model.SysConfig;
using RpcStore.RemoteModel.SysConfig.Model;

namespace RpcStore.Collect
{
    public interface ISysConfigCollect
    {
        void Add (SysConfigAdd config);
        void Delete (SysConfigModel config);
        SysConfigModel Find (string systemType, string key);
        BasicSysConfig FindBasicConfig (long rpcMerId, string name);
        SysConfigModel Get (long id);
        SysConfigBasic[] Query (QuerySysParam query, IBasicPage paging, out int count);
        bool Set (SysConfigModel config, SysConfigSet set);
        bool SetIsEnable (SysConfigModel config, bool isEnable);

        void SetBasicConfig (BasicConfigSet config);
    }
}