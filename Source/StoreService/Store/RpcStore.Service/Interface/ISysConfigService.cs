using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.SysConfig.Model;

namespace RpcStore.Service.Interface
{
    public interface ISysConfigService
    {
        void SetBasicConfig (BasicConfigSet config);
        BasicSysConfig FindBasicConfig (BasicGetParam obj);
        void Add (SysConfigAdd add);
        void Delete (long id);
        SysConfigDatum Get (long id);
        PagingResult<ConfigQueryData> Query (QuerySysParam query, IBasicPage paging);
        void Set (long id, SysConfigSet set);
        void SetIsEnable (long id, bool isEnable);
    }
}